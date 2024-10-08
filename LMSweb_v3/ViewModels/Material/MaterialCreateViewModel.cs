﻿using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels.Material;

public class MaterialCreateViewModel
{
    public string CourseID { get; set; }
    public string CourseName { get; set; }
    public PostData? PostData { get; set; }

}

public class PostData
{
    [Display(Name = "任務編號")]
    public string? MID { get; set; }
    [Display(Name = "任務名稱")]
    public string Name { get; set; }

    [Display(Name = "任務內容")]
    public string Contents { get; set; }

    [Display(Name = "任務開始時間")]
    public string StartDate { get; set; }

    [Display(Name = "任務結束時間")]
    public string EndDate { get; set; }
}