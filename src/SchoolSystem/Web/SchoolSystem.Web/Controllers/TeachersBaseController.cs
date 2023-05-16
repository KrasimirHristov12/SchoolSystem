﻿namespace SchoolSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SchoolSystem.Common;

    [Authorize(Roles = $"{GlobalConstants.Teacher.TeacherRoleName}")]
    public class TeachersBaseController : Controller
    {
    }
}
