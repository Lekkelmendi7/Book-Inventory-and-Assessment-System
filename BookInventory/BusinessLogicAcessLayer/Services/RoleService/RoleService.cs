using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.BusinessLogicAcessLayer.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public RoleService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task CreateRole(RoleCreateModel role)
        {
            var roleModel = new Role
            {
                Name = role.Name,
            };

            if (role.PermissionIds != null && role.PermissionIds.Any())
            {
                List<RolePermission> rolePermissions = new List<RolePermission>();
                foreach (var permission in role.PermissionIds)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = roleModel.Id,
                        PermissionId = permission
                    };
                    rolePermissions.Add(rolePermission);
                }
                roleModel.RolePermissions = rolePermissions;
            }

            _context.Roles.Add(roleModel);
            await _context.SaveChangesAsync();
        }




        public async Task DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return;
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }




        public async Task<RoleGetModel> GetRoleById(int id)
        {
            var role = await _context.Roles.Include(u => u.RolePermissions).ThenInclude(rp => rp.Permission).FirstOrDefaultAsync(s => s.Id == id);
            if (role == null)
            {
                return null;
            }
            return _mapper.Map<RoleGetModel>(role);
        }

        public async Task<PaginatedResult<RoleGetModel>> GetRoles(int page, int size)
        {
            if (size > PaginationModel.MaxPageSize)
            {
                size = PaginationModel.MaxPageSize; // Ensure page size does not exceed max limit
            }

            try
            {
               

                // Define the query
                var queryable = _context.Roles
                    .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .AsQueryable();

                // Apply pagination
                var paginatedRoles = queryable.Paginate(page, size);

                // Map the results
                var mappedRoles = _mapper.Map<IEnumerable<RoleGetModel>>(paginatedRoles.Items);

               

                return new PaginatedResult<RoleGetModel>
                {
                    TotalItems = paginatedRoles.TotalItems,
                    Items = mappedRoles,
                    TotalPages = paginatedRoles.TotalPages,
                    CurrentPage = paginatedRoles.CurrentPage,
                    HasPreviousPage = paginatedRoles.HasPreviousPage,
                    HasNextPage = paginatedRoles.HasNextPage,
                    FirstPage = paginatedRoles.FirstPage,
                    LastPage = paginatedRoles.LastPage
                };
            }
            catch (Exception ex)
            {
               
                throw new Exception("An error occurred while trying to fetch roles.", ex);
            }
        }
        public async Task UpdateRole(int id, RoleUpdateModel model)
        {
            var roleModel = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roleModel == null)
            {
                return;
            }

            roleModel.Name = model.Name;

            _context.RolePermissions.RemoveRange(roleModel.RolePermissions);

            if (model.PermissionIds != null && model.PermissionIds.Any())
            {
                List<RolePermission> rolePermissions = new List<RolePermission>();
                foreach (var permission in model.PermissionIds)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = roleModel.Id,
                        PermissionId = permission
                    };

                    _context.RolePermissions.Add(rolePermission);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
