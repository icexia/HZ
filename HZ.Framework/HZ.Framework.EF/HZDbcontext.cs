using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HZ.Framework.EF
{
    public class HZDbcontext : System.Data.Entity.DbContext
    {
        public HZDbcontext()
            : base("HZDB")  // 不开启 EF SQL 跟踪
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public HZDbcontext(string connectionString)
            : base("HZDB")  // 不开启 EF SQL 跟踪
        {
            this.Database.Connection.ConnectionString = connectionString;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            var context = ((IObjectContextAdapter)this).ObjectContext;
            var objectStateEntries = context.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            foreach (var entry in objectStateEntries)
            {
                Type entityType = entry.Entity.GetType();
                PropertyInfo[] pInfo = entityType.GetProperties();

                for (int i = 0; i < pInfo.Count(); i++)
                {
                    string pName = pInfo[i].Name;
                    Attribute attr = Attribute.GetCustomAttribute(pInfo[i], typeof(System.ComponentModel.DefaultValueAttribute));
                    if (attr != null)
                    {
                        if (entry.CurrentValues[pName] == null || entry.CurrentValues[pName].ToString() == "" || entry.CurrentValues[pName].ToString() == "0")
                        {

                            if (pInfo[i].PropertyType == ((System.ComponentModel.DefaultValueAttribute)attr).Value.GetType())
                                entityType.GetProperty(pInfo[i].Name).SetValue(entry.Entity, ((System.ComponentModel.DefaultValueAttribute)attr).Value, null);
                            else
                            {
                                if (pInfo[i].PropertyType.IsGenericType && pInfo[i].PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                                {
                                    System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(pInfo[i].PropertyType);
                                    entityType.GetProperty(pInfo[i].Name).SetValue(entry.Entity, Convert.ChangeType(((System.ComponentModel.DefaultValueAttribute)attr).Value, nullableConverter.UnderlyingType), null);
                                }
                                else
                                {
                                    entityType.GetProperty(pInfo[i].Name).SetValue(entry.Entity, Convert.ChangeType(((System.ComponentModel.DefaultValueAttribute)attr).Value, pInfo[i].PropertyType), null);
                                }
                            }
                        }
                        else if (entry.CurrentValues[pName] is DateTime && ((DateTime)entry.CurrentValues[pName]).ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00")
                        {
                            entityType.GetProperty(pName).SetValue(entry.Entity, DateTime.Now, null);
                        }
                    }
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 移除EF的表名公约  
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // 还可以移除对MetaData表的查询验证
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            Database.SetInitializer<HZDbcontext>(null);

            // modelBuilder.Entity<TplTemplate>()
            //.HasOptional<TplModuleInfo>(u => u.TplModuleInfo)
            //.WithOptionalDependent(c => c.TplTemplate).Map(p => p.MapKey("PId")).WillCascadeOnDelete(false);

        }

        //具体项目的DB继承该类 添加各自的表映射关系 比如
        //public DbSet<USER> UserList { get; set; }

    }
}
