﻿// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer
{
	[Serializable]
	public class CSharpProject : VSProject
	{
		[XmlIgnore]
		private XmlProjectFile VS2017ProjectFile
		{
			get { return XmlProjectFiles[0]; }
			set { XmlProjectFiles[0] = value; }
		}

		private void Init()
		{
			VS2017ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2017,
				Parent = this,
				Extension = MyEnum.CSProj
			};
		}

		public CSharpProject() : base(1)
		{
			Init();
		}

		public CSharpProject(string path) : base(path, 1)
		{
			Init();
		}

		public override void CreateFiles()
		{
			CreateFiles(MyEnum.TemplateCSharpProj);

			FindProjectFiles();

			CreateProjects();

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCSharp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSProjects();

			FindFiles();
			FindProjectFiles();
		}

		protected override void ReformVSProjXml(XmlProjectFile proj)
		{
			ReformRootNamespace(proj.Xml);
			ReformAssemblyName(proj.Xml);
			ReformCodeFileName(proj.Xml);
			proj.Xml.Save(proj.File.FullName);
		}
	}
}