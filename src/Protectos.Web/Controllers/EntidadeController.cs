using Protectos.Application.Interfaces.Entidades;
using Protectos.Application.ViewModels.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Protectos.Web.Controllers
{
  

    public class EntidadeController : Controller
    {
        private readonly string caminhoImagens = ConfigurationManager.AppSettings["Imagens"];

        private readonly IEntidadeApplicationService _entidadeApplicationService;

        public EntidadeController(IEntidadeApplicationService entidadeApplicationService)
        {
            _entidadeApplicationService = entidadeApplicationService;
        }
        public ActionResult Index()
        {
            return View(_entidadeApplicationService.EntidadeObterAtivo());
        }
        public ActionResult Detalhe(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entidadeViewModel = _entidadeApplicationService.EntidadeObterPorId(id.Value);
            if (entidadeViewModel == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Detalhe", entidadeViewModel);
        }
        public ActionResult Incluir()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(EntidadeViewModel entidadeViewModel)
        {

            if (ModelState.IsValid)
            {
                var entidade = _entidadeApplicationService.EntidadeAdicionar(entidadeViewModel);

                if (!entidade.ValidationResult.IsValid)
                {
                    foreach (var erro in entidade.ValidationResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                    }
                    return View(entidade);
                }
                if (entidade.ValidationResult.ToString() != string.Empty)
                {
                    ViewBag.Sucesso = entidade.ValidationResult.Errors.ToList();
                    return View(entidade);
                }
                return RedirectToAction("Index");
            }
            return View(entidadeViewModel);
        }
        public ActionResult Editar(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entidadeViewModel = _entidadeApplicationService.EntidadeObterPorId(id.Value);
            if (entidadeViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntidadeId = id;
            return View(entidadeViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(EntidadeViewModel entidadeViewModel)
        {
            if (ModelState.IsValid)
            {
                _entidadeApplicationService.EntidadeAtualizar(entidadeViewModel);
                return RedirectToAction("Index");
            }
            return View(entidadeViewModel);
        }
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entidadeViewModel = _entidadeApplicationService.EntidadeObterPorId(id.Value);
            if (entidadeViewModel == null)
            {
                return HttpNotFound();
            }
            return View(entidadeViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            _entidadeApplicationService.DeleteEntidade(id);
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _entidadeApplicationService.Dispose();
            }
            base.Dispose(disposing);
        }

        //Endereco       
        public ActionResult ListarEnderecos(Guid id)
        {
            ViewBag.EntidadeId = id;
            return PartialView("_EnderecosList", _entidadeApplicationService.EntidadeObterPorId(id).Enderecos);
        }
        public ActionResult AdicionarEndereco(Guid id)
        {
            ViewBag.EntidadeId = id;
            return PartialView("_AdicionarEndereco");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarEndereco(EntidadeEnderecoViewModel enderecoViewModel)
        {
            if (ModelState.IsValid)
            {
                _entidadeApplicationService.EntidadeEnderecoAdicionar(enderecoViewModel);
                string url = Url.Action("ListarEnderecos", "Entidade", new { id = enderecoViewModel.EntidadeId });
                return Json(new { success = true, url = url });
            }

            return PartialView("_AdicionarEndereco", enderecoViewModel);
        }
        public ActionResult AtualizarEndereco(Guid id)
        {
            return PartialView("_AtualizarEndereco", _entidadeApplicationService.EntidadeEnderecoObterPorId(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AtualizarEndereco(EntidadeEnderecoViewModel enderecoViewModel)
        {
            if (ModelState.IsValid)
            {
                _entidadeApplicationService.EntidadeEnderecoAtualizar(enderecoViewModel);

                string url = Url.Action("ListarEnderecos", "Entidade", new { id = enderecoViewModel.EntidadeId });
                return Json(new { success = true, url = url });
            }

            return PartialView("_AtualizarEndereco", enderecoViewModel);
        }
        public ActionResult DeletarEndereco(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enderecoViewModel = _entidadeApplicationService.EntidadeEnderecoObterPorId(id.Value);
            if (enderecoViewModel == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DeletarEndereco", enderecoViewModel);
        }
        [HttpPost, ActionName("DeletarEndereco")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarEnderecoConfirmed(Guid id)
        {
            var entidadeId = _entidadeApplicationService.EntidadeEnderecoObterPorId(id).EntidadeId;
            _entidadeApplicationService.DeleteEntidadeEndereco(id);
            string url = Url.Action("ListarEnderecos", "Entidade", new { id = entidadeId });
            return Json(new { success = true, url = url });
        }


        //Email       
        public ActionResult ListarEmails(Guid id)
        {
            ViewBag.EntidadeId = id;
            return PartialView("_EmailsList", _entidadeApplicationService.EntidadeObterPorId(id).Emails);
        }
        public ActionResult AdicionarEmail(Guid id)
        {
            ViewBag.EntidadeId = id;
            return PartialView("_AdicionarEmail");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarEmail(EntidadeEmailViewModel emailViewModel)
        {
            if (ModelState.IsValid)
            {
                _entidadeApplicationService.EntidadeEmailAdicionar(emailViewModel);
                string url = Url.Action("ListarEmails", "Entidade", new { id = emailViewModel.EntidadeId });
                return Json(new { success = true, url = url });
            }

            return PartialView("_AdicionarEmail", emailViewModel);
        }
        public ActionResult AtualizarEmail(Guid id)
        {
            return PartialView("_AtualizarEmail", _entidadeApplicationService.EntidadeEmailObterPorId(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AtualizarEmail(EntidadeEmailViewModel emailViewModel)
        {
            if (ModelState.IsValid)
            {
                _entidadeApplicationService.EntidadeEmailAtualizar(emailViewModel);

                string url = Url.Action("ListarEmails", "Entidade", new { id = emailViewModel.EntidadeId });
                return Json(new { success = true, url = url });
            }

            return PartialView("_AtualizarEmail", emailViewModel);
        }
        public ActionResult DeletarEmail(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var emailViewModel = _entidadeApplicationService.EntidadeEmailObterPorId(id.Value);
            if (emailViewModel == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DeletarEmail", emailViewModel);
        }
        [HttpPost, ActionName("DeletarEmail")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarEmailConfirmed(Guid id)
        {
            var entidadeId = _entidadeApplicationService.EntidadeEmailObterPorId(id).EntidadeId;
            _entidadeApplicationService.DeleteEntidadeEmail(id);
            string url = Url.Action("ListarEmails", "Entidade", new { id = entidadeId });
            return Json(new { success = true, url = url });
        }




        public ActionResult ObterImagemCliente(Guid id)
        {
            //var root = caminhoImagens;
            var foto = Directory.GetFiles(caminhoImagens, id + "*").FirstOrDefault();
            if (foto != null && !foto.StartsWith(caminhoImagens))
            {
                // Validando qualquer acesso indevido além da pasta permitida
                throw new HttpException(403, "Acesso Negado");
            }
            if (foto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return File(foto, "image/jpeg");
        }
    }



}