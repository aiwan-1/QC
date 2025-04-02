using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class SOShipmentCreditHold : PXCacheExtension<SOShipment>
    {

		

		#region UsrCreditHold
		public abstract class usrCreditHold : BqlBool.Field<usrCreditHold> { }

		[PXDBBool]
		[PXUIField(DisplayName = "Credit Hold", Enabled = false, Visible = false)]
		[PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
		public virtual bool? UsrCreditHold
		{
			get;
			set;
		}
		#endregion

		public class MyEvents : PXEntityEvent<SOShipment>.Container<MyEvents>
		{
			public PXEntityEvent<SOShipment> CreditLimitViolated;
			public PXEntityEvent<SOShipment> CreditLimitSatisfied;
		}

	}
}
