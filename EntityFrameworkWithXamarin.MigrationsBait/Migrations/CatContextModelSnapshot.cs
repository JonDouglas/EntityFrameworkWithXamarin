using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EntityFrameworkWithXamarin.MigrationsBait;

namespace EntityFrameworkWithXamarin.MigrationsBait.Migrations
{
    [DbContext(typeof(CatContext))]
    partial class CatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("EntityFrameworkWithXamarin.MigrationsBait.Cat", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MeowsPerSecond");

                    b.Property<string>("Name");

                    b.HasKey("CatId");

                    b.ToTable("Cats");
                });
        }
    }
}
