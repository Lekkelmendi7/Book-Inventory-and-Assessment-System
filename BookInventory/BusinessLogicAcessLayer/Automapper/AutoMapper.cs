using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Models.AccountModels;
using BookInventory.BusinessLogicAcessLayer.Models.PermissionModels;
using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Models.BookModels;

namespace BookInventory.LogicAcessLayer.Automapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Mapping Author entity to models and vice versa!

            CreateMap<Author, AuthorGetModel>();
            CreateMap<AuthorCreateModel, Author>();
            CreateMap<AuthorUpdateModel, Author>();

            //Mapping Book entity to models and vice versa!
            CreateMap<Book, BookGetModel>();
            CreateMap<BookCreateModel, Book>();
            CreateMap<BookUpdateModel, Book>();

            //Mapping Book entity to models and vice versa!
            CreateMap<Publisher, PublisherGetModel>();
            CreateMap<PublisherCreateModel, Publisher>();
            CreateMap<PublisherUpdateModel, Publisher>();


            // Mapping for users
            CreateMap<UserUpdateModel, User>()
                .ForMember(x => x.RoleUsers, options => options.MapFrom(src => MapUsersRoles(src)));
            CreateMap<User, UserGetModel>()
                .ForMember(x => x.Roles, options => options.MapFrom((user, userDTO) => MapUsersRoles(user, userDTO)));

            // Mapping for Roles to Users
            CreateMap<Role, RoleGetModel>()
                .ForMember(x => x.Users, options => options.MapFrom((role, roleDTO) => MapRolesUsers(role, roleDTO)));
            CreateMap<RoleCreateModel, Role>()
                .ForMember(x => x.RoleUsers, options => options.MapFrom(src => MapRolesUsers(src)));
            CreateMap<RoleUpdateModel, Role>()
                .ForMember(x => x.RoleUsers, options => options.MapFrom(src => MapRolesUsers(src)));

            // Map roles to permissions
            CreateMap<Role, RoleGetModel>()
                .ForMember(x => x.Permissions, options => options.MapFrom((role, roleDTO) => MapRolesPermissions(role, roleDTO)));
            CreateMap<RoleCreateModel, Role>()
                .ForMember(x => x.RolePermissions, options => options.MapFrom(src => MapRolesPermissions(src)));
            CreateMap<RoleUpdateModel, Role>()
                .ForMember(x => x.RolePermissions, options => options.MapFrom(src => MapRolesPermissions(src)));

            // Map permissions to roles
            CreateMap<Permission, PermissionGetModel>()
                .ForMember(x => x.Roles, options => options.MapFrom((permission, permissionDTO) => MapPermissionsRoles(permission, permissionDTO)));
            CreateMap<PermissionCreateModel, Permission>()
                .ForMember(x => x.RolePermissions, options => options.MapFrom(src => MapPermissionsRoles(src)));
            CreateMap<PermissionUpdateModel, Permission>()
                .ForMember(x => x.RolePermissions, options => options.MapFrom(src => MapPermissionsRoles(src)));


        }



        private List<RoleGetModel> MapUsersRoles(User user, UserGetModel userDTO)
        {
            var result = new List<RoleGetModel>();

            if (user.RoleUsers != null)
            {
                result.AddRange(user.RoleUsers.Select(roleUser => new RoleGetModel
                {
                    Id = roleUser.Role.Id,
                    Name = roleUser.Role.Name
                }));
            }

            return result;
        }

        private List<RoleUser> MapUsersRoles(UserUpdateModel userUpdateModel)
        {
            var result = new List<RoleUser>();

            if (userUpdateModel.RolesIds != null)
            {
                result.AddRange(userUpdateModel.RolesIds.Select(id => new RoleUser { RoleId = id }));
            }

            return result;
        }

        private List<UserGetModel> MapRolesUsers(Role role, RoleGetModel roleDTO)
        {
            var result = new List<UserGetModel>();

            if (role.RoleUsers != null)
            {
                result.AddRange(role.RoleUsers.Select(roleUser => new UserGetModel
                {
                    Id = roleUser.User.Id,
                    Username = roleUser.User.Username
                }));
            }

            return result;
        }

        private List<RoleUser> MapRolesUsers(RoleCreateModel roleCreationDTO)
        {
            var result = new List<RoleUser>();

            if (roleCreationDTO.UsersIds != null)
            {
                result.AddRange(roleCreationDTO.UsersIds.Select(id => new RoleUser { UserId = id }));
            }

            return result;
        }

        private List<RoleUser> MapRolesUsers(RoleUpdateModel roleUpdateModel)
        {
            var result = new List<RoleUser>();

            if (roleUpdateModel.UsersIds != null)
            {
                result.AddRange(roleUpdateModel.UsersIds.Select(id => new RoleUser { UserId = id }));
            }

            return result;
        }

        private List<PermissionGetModel> MapRolesPermissions(Role role, RoleGetModel roleDTO)
        {
            var result = new List<PermissionGetModel>();

            if (role.RolePermissions != null)
            {
                result.AddRange(role.RolePermissions.Select(rolePermission => new PermissionGetModel
                {
                    Id = rolePermission.Permission.Id,
                    Name = rolePermission.Permission.Name
                }));
            }

            return result;
        }

        private List<RolePermission> MapRolesPermissions(RoleCreateModel roleCreationDTO)
        {
            var result = new List<RolePermission>();

            if (roleCreationDTO.PermissionIds != null)
            {
                result.AddRange(roleCreationDTO.PermissionIds.Select(id => new RolePermission { PermissionId = id }));
            }

            return result;
        }

        private List<RolePermission> MapRolesPermissions(RoleUpdateModel roleUpdateModel)
        {
            var result = new List<RolePermission>();

            if (roleUpdateModel.PermissionIds != null)
            {
                result.AddRange(roleUpdateModel.PermissionIds.Select(id => new RolePermission { PermissionId = id }));
            }

            return result;
        }

        private List<RoleGetModel> MapPermissionsRoles(Permission permission, PermissionGetModel permissionDTO)
        {
            var result = new List<RoleGetModel>();

            if (permission.RolePermissions != null)
            {
                result.AddRange(permission.RolePermissions.Select(rolePermission => new RoleGetModel
                {
                    Id = rolePermission.Role.Id,
                    Name = rolePermission.Role.Name
                }));
            }

            return result;
        }

        private List<RolePermission> MapPermissionsRoles(PermissionCreateModel permissionCreationDTO)
        {
            var result = new List<RolePermission>();

            if (permissionCreationDTO.RoleIds != null)
            {
                result.AddRange(permissionCreationDTO.RoleIds.Select(id => new RolePermission { RoleId = id }));
            }

            return result;
        }

        private List<RolePermission> MapPermissionsRoles(PermissionUpdateModel permissionUpdateModel)
        {
            var result = new List<RolePermission>();

            if (permissionUpdateModel.RoleIds != null)
            {
                result.AddRange(permissionUpdateModel.RoleIds.Select(id => new RolePermission { RoleId = id }));
            }

            return result;
        }
    }
}
