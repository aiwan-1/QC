using PX.Data;
using PX.Data.BQL;
using PX.Objects.AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class AMBatchQCExt : PXCacheExtension<AMBatch>
	{

		#region UsrPressNo
		public abstract class usrPressNo : BqlString.Field<usrPressNo> { }

		[PXDBString(15, IsUnicode = true)]
		[PXUIField(DisplayName = "Press No", Enabled = false, Visible = true)]
		public virtual string UsrPressNo
		{
			get;
			set;
		}
		#endregion

		#region UsrQCCheckNo
		public abstract class usrQCCheckNo : BqlString.Field<usrQCCheckNo> { }

		[PXDBString(15, IsUnicode = true)]
		[PXUIField(DisplayName = "QC Check No", Enabled = false, Visible = true)]
		public virtual string UsrQCCheckNo
		{
			get;
			set;
		}
		#endregion


	}
}
