
using SMTLibReq.DataStructures;
using SMTLibReq.DataStructures.Implementation;
using SMTLibReq.Parsers.BaseExpression;
using SMTLibReq.Parsers.BaseExpression.Interfaces;
using SMTLibReq.Parsers.BooleanExpression;
using SMTLibReq.Parsers.TCTLExpression;
using SMTLibReq.Transformation.BaseTransformationStructures.Interfaces;
using SMTLibReq.Transformation.SMTLib.Transformers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using ProPaS.DataModels;
using ProPaS.DataModels.Interfaces;
using System;
using SMTLibReq.Transformation.SMTLib.Helpers;
using SMTLibReq.Transformation.BaseTransformationStructures;

namespace SMTLibReqConsoleAppForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            string p1 = "AG(p)";
            string p2 = "AG(AG<5(p) => q)"; ;
            string p3 = "AG(p => AF<7(q))";
            string p4 = "AG(p => A(q W<5 r))"; ;
            string p5 = "AG(AG<6(p) => A(q U<5 r))";

            ISystemSpecification tctlSpecification = new SystemSpecification("TCTL");
            IPropertySpecification propertySpecification;
            /*propertySpecification = new PropertySpecification("p1", p1, "");
            tctlSpecification.addOrReplaceProperty(propertySpecification);
            propertySpecification = new PropertySpecification("p2", p2, "");
            tctlSpecification.addOrReplaceProperty(propertySpecification);
            propertySpecification = new PropertySpecification("p3", p3, "");            
            tctlSpecification.addOrReplaceProperty(propertySpecification);
            */
            propertySpecification = new PropertySpecification("p4", p4, "");
            tctlSpecification.addOrReplaceProperty(propertySpecification);
            propertySpecification = new PropertySpecification("p5", p5, "");
            tctlSpecification.addOrReplaceProperty(propertySpecification);
                       
            
            /*
            string pattern = "AG(AG<6(p{0}) => A(q{0} U<5 r{0}))";
            for (int i = 1; i <= 10000; i++) {
                propertySpecification = new PropertySpecification(string.Format("p{0}", i), string.Format(pattern, i), "");
                tctlSpecification.addOrReplaceProperty(propertySpecification);
            }
            */

            var watch = System.Diagnostics.Stopwatch.StartNew();
            ISystemTransformer smtLibTransformer = new SMTLibSystemTransformer();
            ITransformedSystemSpecification transformedSystemSpecification = smtLibTransformer.transform(tctlSpecification);
            watch.Stop();
            System.Diagnostics.Debug.WriteLine(watch.ElapsedMilliseconds / 1000);
            /*foreach (var prop in transformedSystemSpecification.getProperties()) {
                System.Diagnostics.Debug.WriteLine(prop.getProperty());
            }*/

            System.Diagnostics.Debug.WriteLine("test");

        }
}
}
