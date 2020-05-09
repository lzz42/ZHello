using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ZHello.MEF
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var pa = System.AppDomain.CurrentDomain.BaseDirectory;
            //文件系统监控
            //FileSystemWatcher watcher=null;
            //FileWatch(watcher, pa);

            MefTest(pa);

            Console.ReadKey();
        }

        public static void FileWatch(FileSystemWatcher w, string path)
        {
            w = new FileSystemWatcher(path, "*.txt");
            //设置监视类型
            //w.NotifyFilter = NotifyFilters.FileName |
            //                 NotifyFilters.LastWrite ;
            Console.WriteLine("WathcPath:" + path + ";FileType:" + "*.txt");
            w.Changed += (sender, e) =>
            {
                Console.WriteLine("Changed   Name:" + e.Name + ";ChangeType:" + e.ChangeType + ";FullPath:" + e.FullPath);
            };
            w.Created += (s, e) =>
            {
                Console.WriteLine("Created   Name:" + e.Name + ";ChangeType:" + e.ChangeType + ";FullPath:" + e.FullPath);
            };
            w.Deleted += (s, e) =>
            {
                Console.WriteLine("Delete   Name:" + e.Name + ";ChangeType:" + e.ChangeType + ";FullPath:" + e.FullPath);
            };
            w.Renamed += (s, e) =>
            {
                Console.WriteLine("Rename   Name:" + e.Name + ";ChangeType:" + e.ChangeType + ";FullPath:" + e.FullPath);
                Console.WriteLine("Rename   OldName:" + e.OldName + ";OldFullPath:" + e.OldFullPath);
            };
            w.EnableRaisingEvents = true;//开始监视
        }

        private static void MefTest(string pa)
        {
            pa += "..\\";
            var a = new MyClass();
            if (Find(pa, a))
            {
                foreach (var item in a.Cals)
                {
                    var temp = item.GetOperations();
                }
            }
            var b = new MyCal();
            FindExt<MyCal, ICalShow>(pa, b);
            var s = b.Show?.Show(5);
            var c = new MCal2();
            if (Find(pa, c))
            {
                var res3 = c.MDefinC.GetStr();
                var res4 = c.Subtract[0].Value(0.3, 0.4);
                var resi3 = c.Add[0](90, 90);
                var s1 = "1";
                var s2 = "2";
                c.RefeObj(s1, s2);
            }
        }

        /// <summary>
        /// 使用特性标识查找部件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool Find(string path, object obj)
        {
            bool res = false;
            DirectoryCatalog dcl = new DirectoryCatalog(path);
            CompositionContainer cc = new CompositionContainer(dcl);
            try
            {
                cc.ComposeParts(obj);
                res = true;
            }
            catch (ChangeRejectedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        public static async void InitlizeContainer(string path, object obj)
        {
            var catalog = new DirectoryCatalog(path);
            var cb = new CompositionBatch();
            cb.AddPart(obj);
            var container = new CompositionContainer(catalog);
            var minc = new MImC();
            //minc.OmImportsStatisied += (s, e) =>
            //{
            //    Console.WriteLine(e);
            //};
            await Task.Run(() =>
            {
                container.ComposeParts(minc);
                container.Compose(cb);
            });
        }

        public static async void InitlizeContainer<T>(string path, object obj)
            where T : IPartImportsSatisfiedNotification, new()
        {
            var catalog = new DirectoryCatalog(path);
            var cb = new CompositionBatch();
            cb.AddPart(obj);

            var container = new CompositionContainer(catalog);
            var part = new T();
            part.OnImportsSatisfied();
            await Task.Run(() =>
            {
                container.ComposeParts(part);
                container.Compose(cb);
            });
        }

        /// <summary>
        /// 基于约定的部件注册
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool FindEx(string path, MyCal obj)
        {
            var res = false;
            var rb = new RegistrationBuilder();//用于定义出口和入口的约定
            rb.ForTypesDerivedFrom<ICalShow>().Export<ICalShow>();//指定出口约定  给所有实现接口的类型应用[Export(Type)]特性
            rb.ForType<MyCal>().ImportProperty<ICalShow>(t => t.Show);//指定入口约定 将入口映射到指定的属性上
            var dc = new DirectoryCatalog(path, rb);
            CompositionService sv = dc.CreateCompositionService();
            try
            {
                sv.SatisfyImportsOnce(obj, rb); //在目录中搜索部件 多个出口匹配项出异常
                res = true;
            }
            catch (ChangeRejectedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CompositionException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sv.Dispose();
            return res;
        }

        public static bool FindExt<T, K>(string path, T obj)
        {
            return FindExt<T, K>(path, obj, p => p.PropertyType == typeof(K));
        }

        /// <summary>
        /// 基于约定的部件注册 类T注册接口K
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <param name="propertyFilter"></param>
        /// <returns></returns>
        public static bool FindExt<T, K>(string path, T obj, Predicate<PropertyInfo> propertyFilter)
        {
            var res = false;
            var rb = new RegistrationBuilder();//用于定义出口和入口的约定
            rb.ForTypesDerivedFrom<K>().Export<K>();//指定出口约定  给所有实现接口的类型应用[Export(Type)]特性
            rb.ForType<T>().ImportProperties(propertyFilter);//指定入口约定
            var dc = new DirectoryCatalog(path, rb);
            CompositionService sv = dc.CreateCompositionService();
            try
            {
                sv.SatisfyImportsOnce(obj, rb); //在目录中搜索部件 若存在多个出口匹配项则抛出出异常
                res = true;
            }
            catch (ChangeRejectedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CompositionException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sv.Dispose();
            return res;
        }
    }
}

/*  MEF Managed Extensibility Framework
 *  所在命名空间 System.ComponentModel.Composition
 *
 *  宿主->容器->类别->导出部件<--导入部件<-宿主
 *
 *  宿主类：类别和容器
 *  基元类：基类
 *  特性类：基于特性的类
 *
 *  定义约定
 *  1.定义接口
 *  定义实现
 *  定义使用
 */
