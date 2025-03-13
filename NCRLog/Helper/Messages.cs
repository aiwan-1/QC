using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Data.BQL.BqlPlaceholder;

namespace NCRLog
{
   
    [PXLocalizable]
    public static class Messages
    {
        public const string NCR = "Non-Conformance Record";
        public const string ECN = "Engineers Change Notice";

        public const string NCRLog = "Non-Conformance Record Log";
        public const string ECNLog = "Engineers Change Notice Log";

        public const string F = "Full";
        public const string C = "Consignment";
        public const string D = "Deviation";

        public const string ISO = "ISO Records";
        public const string ISOSetup = "ISO Records Setup/Preferences";

        public const string QualityControl = "End of Line Quality Checks";

        public const string H = "HoneyComb";
        public const string R = "Rockwool";
        public const string P = "PIR";
        public const string B = "PlasterBoard";

        public const string MoveNotFound = "Move not Found";
        public const string PressGlueLog = "Press & Glue Log";
        public const string QCDetails = "Quality Control Details";

        public const string NoteIDNotSame = "Note ID cannot be the same the operation has been escaped.";

        public class move : BqlString.Constant<move>
        {
            public move()
                : base("O")
            {
            }
        }

        public const string SOCustomer = "Customer ID does not match, please correct.";

        public const string DocType = "Move and QC Check Document type does not match, please correct. Document type should be Move";

        public const string NoMatchingRecord = "There has not been a record filled in today, please perform the relevant checks";

        public const string New = "New record";

        public const string Avo1 = "Avonmouth Press 1";

        public const string Avo2 = "Avonmouth Press 2";

        public const string Avo3 = "Avonmouth Press 3";

        public const string Npt1 = "Newport Press 1";

        public const string Npt2 = "Newport Press 2";

        public const string Calibration = "Calibration Records";

        public const string DateWrong = "Date is not formatted correctly";

        public const string Inspection = "Inspection Details";

        public const string CalibrationHeader = "Calibration Record Details";

        public const string CalibrationDetail = "Calibration Line Detail";

        public class npdApprover : BqlString.Constant<npdApprover>
        {
            public npdApprover()
                : base("NPDApprover")
            {
            }
        }
    }
}
