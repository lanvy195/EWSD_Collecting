using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using EWSD_Collecting.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        public void Initialize()
        {
            // migration
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            if (_unitOfWork.Department.GetFirstOrDefault(u => u.Name == SD.Role_User_Administrator) == null)
            {
                Department obj = new Department();
                obj.Name = SD.Role_User_Administrator;
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
            }
            if (_unitOfWork.Department.GetFirstOrDefault(u=>u.Name == "Guest") == null)
            {
                Department obj = new Department();
                obj.Name = "Guest";
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
            }
            // create roles default
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Administrator).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Administrator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_QAManager)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_QACoordinator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Staff)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Visitor)).GetAwaiter().GetResult();
            
                //if roles are not created,then we will create admin user as well
                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "admin@test.com",
                        Email = "admin@test.com",
                        FullName = "Administrator",
                        PhoneNumber = "84385190202",
                        DepartmentId = 1,
                        FirstName = "Admin",
                        LastName = "Hiep",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@test.com");
                _userManager.AddToRoleAsync(user, SD.Role_User_Administrator).GetAwaiter().GetResult();
                
                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "manager@test.com",
                        Email = "manager@test.com",
                        FullName = "Lam Hiep",
                        PhoneNumber = "1234567890",
                        DepartmentId = 1,
                        FirstName = "Hiep",
                        LastName = "Lam",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userHiep = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "manager@test.com");
                _userManager.AddToRoleAsync(userHiep, SD.Role_User_QAManager).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "QAmanager@test.com",
                        Email = "QAmanager@test.com",
                        FullName = "Quoc Phat",
                        PhoneNumber = "9874561232",
                        isQA = true,
                        DepartmentId = 2,
                        FirstName = "QA",
                        LastName = "Manager",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userQP = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "QAmanager@test.com");
                _userManager.AddToRoleAsync(userQP, SD.Role_User_QACoordinator).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "staff01@test.com",
                        Email = "staff01@test.com",
                        FullName = "Nguyen Quoc Cuong",
                        PhoneNumber = "1593574862",
                        DepartmentId = 2,
                        FirstName = "Cuong",
                        LastName = "Nguyen",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userNC = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "staff01@test.com");
                _userManager.AddToRoleAsync(userNC, SD.Role_User_Staff).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "staff02@test.com",
                        Email = "staff02@test.com",
                        FullName = "Gia Bao",
                        PhoneNumber = "4568521397",
                        DepartmentId = 2,
                        FirstName = "Bao",
                        LastName = "Gia",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userGB = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "staff02@test.com");
                _userManager.AddToRoleAsync(userGB, SD.Role_User_Staff).GetAwaiter().GetResult();
            }

            return;

        }
    }
}
