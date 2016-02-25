﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA_lab_5
{
    class DefaultCell : BaseCell
    {
        public DefaultCell(double fe, double re, double te, double ae) : base(fe, re, te, ae) { }
        public override Func<double, double> Fullness
        {
            get
            {
                return (t) => Math.Min(this.fullness_expert * (1 + this.Alpha * t), 1);
            }
        }

        public override Func<double, double> Reliability
        {
            get
            {
                return (t) => Math.Min(this.reliability_expert * (1 + this.Gamma * t), 1);
            }
        }

        public override Func<double, double> Timeliness
        {
            get
            {
                return (t) => Math.Max(this.timeliness_expert * (1 - this.Beta * t * t), 1);
            }
        }

        public override DoubleInterval[] FindTimeInterval(double lowerbound, double upperbound)
        {
            throw new NotImplementedException();
        }

        protected override void CalculateCoefficients()
        {
            this.Alpha = this.alpha_expert > 1 ? 0 : Math.Exp(this.alpha_expert) * this.fullness_expert * 0.5;
            this.Gamma = this.alpha_expert > 1 ? 0 : Math.Exp(this.reliability_expert) * this.alpha_expert * 0.05;
            this.Beta = this.alpha_expert > 1 ? 0 : (this.alpha_expert + this.Gamma) * this.timeliness_expert * 1e-5;
        }

        public static DefaultCell CreateInstance(double fe, double re, double te, double ae)
        {
            return new DefaultCell(fe, re, te, ae);
        }
    }

    class VariantCell : BaseCell
    {
        public VariantCell(double fe, double re, double te, double ae) : base(fe, re, te, ae) { }
        public override Func<double, double> Fullness
        {
            get
            {
                return (t) => Math.Min(this.fullness_expert * (1 + 0.5 * (1 + this.Alpha) * t), 1);
            }
        }

        public override Func<double, double> Reliability
        {
            get
            {
                return (t) => Math.Min(this.reliability_expert * (1 + (1 + this.Gamma) * t) * 1e-2, 1);
            }
        }

        public override Func<double, double> Timeliness
        {
            get
            {
                return (t) => Math.Min(0.5 * this.timeliness_expert * (2 - this.Beta * t) * (2 - this.Beta * t), 1);
            }
        }

        public override DoubleInterval[] FindTimeInterval(double lowerbound, double upperbound)
        {
            throw new NotImplementedException();
        }

        protected override void CalculateCoefficients()
        {
            // this must be changed after task resolve
            this.Alpha = this.alpha_expert > 1 ? 0 : Math.Exp(this.alpha_expert) * this.fullness_expert * 0.5;
            this.Gamma = this.alpha_expert > 1 ? 0 : Math.Exp(this.reliability_expert) * this.alpha_expert * 0.05;
            this.Beta = this.alpha_expert > 1 ? 0 : (this.alpha_expert + this.Gamma) * this.timeliness_expert * 1e-5;
        }
        public static VariantCell CreateInstance(double fe, double re, double te, double ae)
        {
            return new VariantCell(fe, re, te, ae);
        }
    }

    class CustomCell : BaseCell
    {
        public CustomCell(double fe, double re, double te, double ae) : base(fe, re, te, ae) { }
        public override Func<double, double> Fullness
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Func<double, double> Reliability
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Func<double, double> Timeliness
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override DoubleInterval[] FindTimeInterval(double lowerbound, double upperbound)
        {
            throw new NotImplementedException();
        }

        protected override void CalculateCoefficients()
        {
            throw new NotImplementedException();
        }
        public static CustomCell CreateInstance(double fe, double re, double te, double ae)
        {
            return new CustomCell(fe, re, te, ae);
        }
    }
}