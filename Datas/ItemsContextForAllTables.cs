using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Datas;
using FinalProject.Datas.Edintites;
using Microsoft.EntityFrameworkCore;


namespace FinalProject.Datas
{
    public class ItemsContextForAllTables : DbContext
    {
        public ItemsContextForAllTables(DbContextOptions<ItemsContextForAllTables> options) : base(options)
        {

        }
        public virtual DbSet<Category> Category { get; set; } //map items table
        public virtual DbSet<Cart> Cart { get; set; } //map Users table
        public virtual DbSet<Images> Images { get; set; } //map Users table
        public virtual DbSet<Order> Order { get; set; } //map Users table
        public virtual DbSet<OrderDetails> OrderDetails { get; set; } //map Users table
        public virtual DbSet<Products> Product { get; set; } //map Users table
        public virtual DbSet<Users> Users { get; set; } //map Users table
        public virtual DbSet<CartProduct> CartProduct { get; set; } //map Users table
        public virtual DbSet<Brands> Brands { get; set; } //map Users table
        public virtual DbSet<Sliders> Slider { get; set; } //map Users table


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-MEBLLCM;Database=StoreOnline;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Products>().HasOne<Category>(P => P.category).WithMany(C => C.products).HasForeignKey(C=>C.categoryId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Products>().HasOne<Brands>(P => P.brand).WithMany(B => B.products).HasForeignKey(B=>B.brandId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Images>().HasOne<Products>(I => I.products).WithMany(P => P.Image).HasForeignKey(IP => IP.productId).OnDelete(DeleteBehavior.ClientCascade);


            modelBuilder.Entity<Order>().HasMany<OrderDetails>(O => O.orderDetails).WithOne(OD => OD.order).HasForeignKey(B => B.orderId).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Users>().HasMany<Order>(U => U.order).WithOne(OU => OU.user).HasForeignKey(UO => UO.userId).OnDelete(DeleteBehavior.ClientCascade);


            modelBuilder.Entity<Users>()
            .HasOne<Cart>(u => u.CartUser)
            .WithOne(C => C.users)
            .HasForeignKey<Cart>(CU => CU.usersid);


            modelBuilder.Entity<CartProduct>().HasKey(CP => new { CP.cartId, CP.productId});

            modelBuilder.Entity<CartProduct>()
                .HasOne<Cart>(CC => CC.cart)
                .WithMany(CP => CP.cartProducts)
                .HasForeignKey(CP => CP.cartId);


            modelBuilder.Entity<CartProduct>()
                .HasOne<Products>(P => P.products)
                .WithMany(CP => CP.cartProducts)
                .HasForeignKey(CP => CP.productId);

        }


    }
}
