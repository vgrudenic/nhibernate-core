using System.Xml;

namespace NHibernate.Cfg.XmlHbmBinding
{
	public class MappingRootBinder : Binder
	{
		private readonly Dialect.Dialect dialect;

		public MappingRootBinder(Mappings mappings, XmlNamespaceManager namespaceManager,
			Dialect.Dialect dialect)
			: base(mappings, namespaceManager)
		{
			this.dialect = dialect;
		}

		public void Bind(XmlNode node)
		{
			mappings.SchemaName = GetAttributeValue(node, "schema") ?? null;
			mappings.DefaultCascade = GetAttributeValue(node, "default-cascade") ?? "none";
			mappings.DefaultAccess = GetAttributeValue(node, "default-access") ?? "property";
			mappings.DefaultLazy = "true".Equals(GetAttributeValue(node, "default-lazy") ?? "true");
			mappings.IsAutoImport = "true".Equals(GetAttributeValue(node, "auto-import") ?? "true");
			mappings.DefaultNamespace = GetAttributeValue(node, "namespace") ?? mappings.DefaultNamespace;
			mappings.DefaultAssembly = GetAttributeValue(node, "assembly") ?? mappings.DefaultAssembly;

			new FilterDefBinder(this).BindEach(node, HbmConstants.nsFilterDef);
			new RootClassBinder(this, dialect).BindEach(node, HbmConstants.nsClass);
			new SubclassBinder(this, dialect).BindEach(node, HbmConstants.nsSubclass);
			new JoinedSubclassBinder(this, dialect).BindEach(node, HbmConstants.nsJoinedSubclass);
			new NamedQueryBinder(this).BindEach(node, HbmConstants.nsQuery);
			new NamedSQLQueryBinder(this).BindEach(node, HbmConstants.nsSqlQuery);
			new ImportBinder(this).BindEach(node, HbmConstants.nsImport);
			new AuxiliaryDatabaseObjectBinder(this).BindEach(node, HbmConstants.nsDatabaseObject);
			new ResultSetMappingDefinitionBinder(this).BindEach(node, HbmConstants.nsResultset);
		}
	}
}