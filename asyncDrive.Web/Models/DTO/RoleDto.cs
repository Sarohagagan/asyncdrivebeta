namespace asyncDrive.Web.Models.DTO
{
    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
    public class UpdateUserRolesDto
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
