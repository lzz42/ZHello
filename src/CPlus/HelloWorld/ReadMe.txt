
C++ 动态库示例
以使用crpycpp为例：

1.使用外部C++库；
	使用动态库：
	外部库头文件，lib文件，dll文件
	添加头文件：项目--属性--c/c++--附件包含目录=》添加头文件目录；
	添加lib文件：项目--属性--链接器--常规--附加库目录=》添加lib库文件目录；
	指定礼拜文件：项目--属性--链接器--输入--附加依赖项=》添加依赖的lib文件名；
	若要调戏需要生成完整的程序数据库文件：项目--属性--链接器--调试--生成完整的程序数据库文件：是；

2.自定义DLL功能；
	
3.添加导出函数；
	使用 extern "C" __declspec(dllexport) 标记要导出的函数
	eg:
	extern "C" __declspec(dllexport)
	int Test(int a)
	{
		return a*100;
	}
	使用.def模块文件
	使用Depends工具查看导出的函数有哪些；

4.生成动态库；
	自动复制脚本 --- 在项目生成时自动复制相关依赖库
	使用Depends工具查看依赖库；
	若仍然缺少依赖库，使用processmonitor工具进行诊断；
	后期生成事件添加：
	copy "$(ProjectDir)lib\x64\*.*" "$(OutputPath)*.*"

5.外部调用时，进行调试
	附加进行调试
	若要调试C++库，需要将对于的pdb文件复制到项目路径下，然后进行附加调试

6.C#中使用：
	[DllImport(DLLPATH, EntryPoint = "GetNdiName")]
    public static extern void GetNdiName(IntPtr sources,ref int count);
-----------------------------------------------------------------------
-----------------------------------------------------------------------
-----------------------------------------------------------------------

Mark:

7.C++函数返回值或者输出项
	1.单个值类型/结构体
		值类型：在C#定义兼容类型；
		结构体：在C#定义对应的结构体：(数据结构的内存空间需要在C#逻辑中申请)
			C++函数定义 Fun(struct * s,int * i);//输出结构体s和整形i；
			C#函数调用 Fun(IntPtr s,ref int i);
	2.返回数组类型：
	//定义数据结构
	[StructLayoutAttribute(LayoutKind.Sequential)]//指定数据结构内存分布为顺序存储布局
    public struct NDIlib_source_t
    {   
        public IntPtr p_ndi_name;//字符串类型
        public IntPtr p_ip_address;//字符串类型
    }
		1.首先需要确定返回值的空间大小，即数组元素大小*元素个数：
			元素大小：
			eg：int size = Marshal.SizeOf(typeof(NDIlib_source_t));
				int totalsize = count*size;
		2.给指针分配空间：
			eg: IntPtr data = Marshal.AllocHGlobal(total);
		3.调用C++函数，将返回值数据放到C#指定的指针空间中；
		4.读取数据并转换为C#中定义的数据类型：
			data指针偏移一个数据结构：
			InPtr res = IntPtr.Add(data, size);
			//类型转化
			var src = (NDIlib_source_t)Marshal.PtrToStructure(res, typeof(NDIlib_source_t))

	8.C++使用第三方库的方式：
	静态库(.a,.lib)与动态库(.so,.dll)区别：
	区别依据：
	a.引用该库的静态库（static）-Static；
	b.引用库的动态库（dll）-DLL-ONLY；
	c.同时使用静态库和动态库-DLL-Import,此时工程运行库引入（MT/MTD）；


