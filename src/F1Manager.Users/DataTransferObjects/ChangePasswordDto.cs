﻿namespace F1Manager.Users.DataTransferObjects;

public class ChangePasswordDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}