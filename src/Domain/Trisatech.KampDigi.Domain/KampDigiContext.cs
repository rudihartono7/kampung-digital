using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Domain
{
    public class KampDigiContext: DbContext
    {
        public KampDigiContext(DbContextOptions<KampDigiContext> options)
            :base(options)
        {

        }
        /// <summary>
        /// Residence = kampung
        /// </summary>
        public virtual DbSet<Residence> Residences { get; set; }
        /// <summary>
        /// Table untuk menyimpan data Rumah
        /// </summary>
        public virtual DbSet<House> Houses { get; set; }
        /// <summary>
        /// Table untuk menyimpan data warga
        /// </summary>
        public virtual DbSet<Resident> Residents { get; set; }
        /// <summary>
        /// Table untuk menyimpan data anggota keluarga
        /// </summary>
        public virtual DbSet<ResidentFamily> ResidentFamilies { get; set; }
        /// <summary>
        /// Table untuk menyimpan data tamu
        /// </summary>
        public virtual DbSet<GuestBook> GuestBooks { get; set; }
        /// <summary>
        /// Table untuk menyimpan informasi keuangan
        /// </summary>
        public virtual DbSet<ResidentFund> ResidentFunds { get; set; }
        /// <summary>
        /// Table untuk menyimpan data iuran
        /// </summary>
        public virtual DbSet<ResidentBill> ResidentBills { get; set; }
        /// <summary>
        /// Table untuk menyimpan data kegiatan
        /// </summary>
        public virtual DbSet<ResidentProgram> ResidentPrograms { get; set; }
        /// <summary>
        /// Table untuk menyimpan data forum
        /// </summary>
        public virtual DbSet<Post> Posts { get; set; }
        /// <summary>
        /// Table untuk menyimpan data komentar
        /// </summary>
        public virtual DbSet<Comment> Comments { get; set; }
        /// <summary>
        /// Table untuk menyimpan data pengumuman
        /// </summary>
        public virtual DbSet<Publication> Publications { get; set; }
        /// <summary>
        /// Table untuk menyimpan data donasi
        /// </summary>
        public virtual DbSet<Donation> Donations { get; set; }
        /// <summary>
        /// Digunakan untuk menyimpan data login
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
        /// <summary>
        /// Untuk menyimpan transaksi keuangan
        /// </summary>
        public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }
        public virtual DbSet<ResidentBillBaseInfo> ResidentBillBaseInfos { get; set; }
        public virtual DbSet<ResidentBillDetailInfo> ResidentBillDetailInfos { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
        }
    }
}
