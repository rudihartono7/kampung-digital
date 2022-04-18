using System;
using System.Globalization;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models;
public class ResidentBillModel
{
    public ResidentBillModel()
    {   
    }
    
    public Guid Id { get; set; }
    public Guid ResidentTo { get; set; }
    public string NameResident { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Nominal { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime BillOpenDate { get; set; }
    public string Evidence { get; set; }
    public string Note { get; set; }
    public bool Status { get; set; }

}