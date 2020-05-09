# 不使用Costura Nuget包 构建Costura打包工程

## 第一步 复制文件

- 复制文件到要构建的工程内(不含FodyWeavers.xml,FodyWeavers.xsd)

## 第二步 编辑工程

- 工程添加DLL引用：Costura.dll
- 工程添加FodyWeavers.xml文件到Root
- 编辑工程csproj
  - 头部位置添加：`<Import Project="Build\build\Costura.Fody.props" Condition="Exists('Build\build\Costura.Fody.props')" />`
  - 尾部位置添加：`<Import Project="Build\build\Fody.targets" Condition="Exists('Build\build\Fody.targets')" />`

## 第三步 配置规则

- 根据Costura规则，编辑FodyWeavers.xml

## 第四步 生成工程

- 查看输出面板是否有异常信息

## 第五步 验证结果

- 反编译DLL或者Exe查看：
  - 1.Resources是否具有指定的文件
  - 2.是否含有Costura命名空间
  - 3.`<Model>`静态构造函数是否已添加来自Costurade初始化方法：`AssemblyLoader.Attach();`
- 运行EXE或者调用DLL进行测试
