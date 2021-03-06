﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bank.BusinessLogic;
using Bank.Commons.Concretes.Helpers;
using Bank.Commons.Concretes.Logger;
using Bank.Models.Concretes;

namespace BankAppWeb.Controllers
{
    public class CustomerController : Controller
    {

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                // TODO: Add insert logic

                InsertCustomer(
                    collection["CustomerName"],
                    collection["CustomerSurname"],
                    collection["CustomerPasskey"],
                    decimal.Parse(collection["Balance"]),
                    byte.Parse(collection["BalanceType"]));
                
                return RedirectToAction("ListAll");
            }
            catch(Exception ex)
            {
                LogHelper.Log(LogTarget.File,ExceptionHelper.ExceptionToString(ex),true);
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var customer = SelectCustomerByID(id);
                return View(customer);
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                UpdateCustomer(
                    collection["CustomerName"],
                    collection["CustomerSurname"],
                    collection["CustomerPasskey"],
                    decimal.Parse(collection["Balance"]),
                    byte.Parse(collection["BalanceType"]),
                    bool.Parse(collection["isActive"]));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                DeleteCustomer(id);

                return RedirectToAction("ListAll");
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Operation failed!", ex);
            }
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("ListAll");
            }
            catch(Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Operation failed!", ex);
            }
        }

        public ActionResult ListAll()
        {
            try
            {
                var customer = from c in ListAllCustomers()
                                     orderby c.CustomerID
                                     select c;
                return View(customer);
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        #region PRIVATE METHODS
        
        private void InsertCustomer(string name, string surname, string passkey, decimal balance, byte balancetype)
        {
            try
            {
                using (var business = new CustomersBusiness())
                {
                    Customers customer = new Customers();
                    customer.CustomerName = name;
                    customer.CustomerSurname = surname;
                    customer.CustomerPasskey = passkey;
                    customer.Balance = balance;
                    customer.BalanceType = balancetype;
                    customer.isActive = true;

                    bool success = business.InsertCustomer(customer);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private void UpdateCustomer(string name, string surname, string passkey, decimal balance, byte balancetype, bool isActive)
        {
            try
            {
                using (var business = new CustomersBusiness())
                {
                    var entity = new Customers();
                    entity.CustomerName = name;
                    entity.CustomerSurname = surname;
                    entity.CustomerPasskey = passkey;
                    entity.Balance = balance;
                    entity.BalanceType = balancetype;
                    entity.isActive = isActive;

                    var success = business.UpdateCustomer(entity);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private void DeleteCustomer(int ID)
        {
            try
            {
                using (var business = new CustomersBusiness())
                {
                    business.DeleteCustomerById(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private List<Customers> ListAllCustomers()
        {
            try
            {
                using (var business = new CustomersBusiness())
                {
                    return business.SelectAllCustomers();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private Customers SelectCustomerByID(int ID)
        {
            try
            {
                using (var business = new CustomersBusiness())
                {
                    return business.SelectCustomerById(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File,ExceptionHelper.ExceptionToString(ex),true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        #endregion
    }
}
