﻿using ClosedXML.Excel;
using EmprestimosLivros.Data;
using EmprestimosLivros.Models;
using EmprestimosLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmprestimosLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ISessaoInterface _sessaoInterface;

        public EmprestimoController(ApplicationDbContext db, ISessaoInterface sessaoInterface)
        {
            _db = db;
            _sessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if(usuario == null)
            {
                return RedirectToAction("Login", "login");
            }

            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;
            return View(emprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel emprestimo)
        {
            if(ModelState.IsValid)
            {
                emprestimo.DataUltimaAtualizacao = DateTime.Now;

                _db.Emprestimos.Add(emprestimo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "login");
            }

            if (id == null || id==0)
            {
                return NotFound();
            }

            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult Editar(EmprestimosModel emprestimo)
        {
            if(ModelState.IsValid)
            {
                var emprestimoDB = _db.Emprestimos.Find(emprestimo.Id);

                emprestimoDB.Fornecedor = emprestimo.Fornecedor;
                emprestimoDB.Recebedor = emprestimo.Recebedor;
                emprestimoDB.LivroEmprestado = emprestimo.LivroEmprestado;

                _db.Emprestimos.Update(emprestimoDB);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";
                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Algum erro ocorreu ao realizar a edição!";

            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "login");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if(emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        public IActionResult Exportar()
        {
            var dados = GetDados();

            using (XLWorkbook workBook = new XLWorkbook()) 
            {
                workBook.AddWorksheet(dados, "Dados Empréstimo");

                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Emprestimo.xls");
                }
            }
        }

        private DataTable GetDados()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = "Dados empréstimos";
            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("Data empréstimo", typeof(DateTime));

            var dados = _db.Emprestimos.ToList();

            if(dados.Count > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataUltimaAtualizacao);
                });
            }

            return dataTable;
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel emprestimo)
        {
            if(emprestimo == null)
            {
                return NotFound();
            }

            _db.Emprestimos.Remove(emprestimo);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Remoção realizada com sucesso!";


            return RedirectToAction("Index");
        }

    }
}
