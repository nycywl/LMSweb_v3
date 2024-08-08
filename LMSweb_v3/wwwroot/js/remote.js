let local_user_message_template, remote_user_message_template, waiting_message_template;
let dialogue, commentObj;

const getGPTMessage = async (question) => {
    document.querySelector('input').setAttribute('disabled', "");
    document.querySelector('#submit-btn').setAttribute('disabled', "");
    let btns = document.querySelectorAll('.remote:not(:last-child) .satisfied');
    btns.forEach(btn => {
        if (!btn.classList.contains('hidden')) {
            btn.classList.add('hidden');
        }
    });
    let resp = await fetch(PRO_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(question),
        cache: 'no-cache',
    });
    let chat_message = await resp.json();
    if (resp.status !== 200) {
        throw {
            status: resp.status,
            message: chat_message.message
        };
    }
    document.querySelector('.remote:last-child .text').textContent = chat_message.answer;
    dialogue.scrollTo(dialogue.scrollTop, dialogue.scrollHeight);
    document.querySelector('input').removeAttribute('disabled');
    document.querySelector('#submit-btn').removeAttribute('disabled');
};

/**
 * 測試用
 * @param {string} question
 */
const getMockGPTMessage = (question) => {
    document.querySelector('.remote:last-child .satisfied').classList.remove('hidden');
    document.querySelector('.remote:last-child .text').textContent = question;
    dialogue.scrollTo(dialogue.scrollTop, dialogue.scrollHeight);
}

const renderLocalUserMessage = (inputMsg) => {
    let localUserMessage = local_user_message_template.content.cloneNode(true);
    localUserMessage.querySelector('.local .text').innerHTML = inputMsg;
    return localUserMessage;
};

const renderRemoteWaitingMessage = () => {
    let waitingMessage = waiting_message_template.content.cloneNode(true);
    let remoteUserMessage = remote_user_message_template.content.cloneNode(true);
    remoteUserMessage.querySelector('.remote .text').innerHTML = "";
    remoteUserMessage.querySelector('.remote .text').append(waitingMessage);
    return remoteUserMessage;
};

const submit_process = (e) => {
    e.preventDefault();
    let input_question = document.querySelectorAll('input');
    let msgHistory = document.querySelectorAll('.user .text');
    let msgHistoryArr = [];
    for (let item of msgHistory) {
        msgHistoryArr.push(item.textContent);
    }
    msgHistoryArr.shift();
    let dataObj = {
        question: input_question[0].value,
        user_id: input_question[1].value,
        course_id: input_question[2].value,
        history: msgHistoryArr
    };
    let node_local_user = renderLocalUserMessage(dataObj.question);
    dialogue.append(node_local_user, renderRemoteWaitingMessage());
    dialogue.scrollTo(dialogue.scrollTop, dialogue.scrollHeight);
    getGPTMessage(dataObj).catch(err => {
        if (err.status === 500) {
            document.querySelector('.remote:last-child .text').textContent = "系統錯誤，請稍後再試";
        } else if (err.status === 400) {
            document.querySelector('.remote:last-child .text').textContent = "請輸入問題";
        } else if (err.status === 404) {
            document.querySelector('.remote:last-child .text').textContent = "找不到答案，請重新輸入";
        }
        input_question[0].removeAttribute('disabled');
        document.querySelector('#submit-btn').removeAttribute('disabled');
    });
    input_question[0].value = "";
};

const test_process = (e) => {
    e.preventDefault();
    alert("test");
};

window.onload = () => {
    local_user_message_template = document.querySelector('#local_user');
    remote_user_message_template = document.querySelector('#remote_user');
    waiting_message_template = document.querySelector('#waiting');
    dialogue = document.querySelector('.dialogue');
    let form = document.querySelector('#input-question');
    form.addEventListener('submit', submit_process);
};
