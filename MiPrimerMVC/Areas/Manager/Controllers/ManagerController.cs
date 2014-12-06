using System.Web.Mvc;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Areas.Manager.Models;

namespace MiPrimerMVC.Areas.Manager.Controllers
{
    public class ManagerController: Controller
    {

        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        //readonly IMappingEngine _mappingEngine;
        public ManagerController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository/*, IMappingEngine mappingEngine*/)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
           // _mappingEngine = mappingEngine;
        }

        // GET: /Admin/Manage/Index
        public ActionResult ManagerView()
        {
            return View();
        }

        public ActionResult ManageUsers(long id)
        {
            var managerModel = new ManagerModel();
            managerModel.UserToBeArchived = _readOnlyRepository.FirstOrDefault<AccountLogin>(x=>x.Id==id);

            if (!managerModel.UserToBeArchived.Archived)
            {
                managerModel.UserToBeArchived.Archived = true;
            }
            else
            {
                managerModel.UserToBeArchived.Archived = false;
            }

            _writeOnlyRepository.Update(managerModel.UserToBeArchived);

            return View(managerModel);
        }

    }
}