-- 更新T001的教師名稱為 林廣學老師
UPDATE Teachers
SET TeacherName = '林廣學 老師'
WHERE TeacherID = 'T001';

-- 更新T002的教師名稱為 洪子秀老師
UPDATE Teachers
SET TeacherName = '洪子秀 老師'
WHERE TeacherID = 'T002';

-- 更新T003的教師名稱為 曾秋蓉老師
UPDATE Teachers
SET TeacherName = '曾秋蓉 老師'
WHERE TeacherID = 'T003';

-- 更新T004的教師名稱為 李偉老師
UPDATE Teachers
SET TeacherName = '李偉 老師'
WHERE TeacherID = 'T004';

-- 更新T005的教師名稱為 蔡老師
UPDATE Teachers
SET TeacherName = '蔡 老師'
WHERE TeacherID = 'T005';
SELECT * FROM Teachers;
