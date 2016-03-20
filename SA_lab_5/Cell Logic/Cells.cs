﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA_lab_5.Cell_Logic
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
                return (t) => Math.Max(this.timeliness_expert * (1 - this.Beta * t * t), 0);
            }
        }

        protected override void CalculateCoefficients()
        {
            this.Alpha = this.alpha_expert > 1 ? 0 : Math.Exp(this.alpha_expert) * this.fullness_expert * 0.5;
            this.Gamma = this.alpha_expert > 1 ? 0 : Math.Exp(this.reliability_expert) * this.alpha_expert * 0.05;
            this.Beta = this.alpha_expert > 1 ? 0 : (this.alpha_expert + this.Gamma) * this.timeliness_expert * 1e-5;
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
                return (t) => Math.Max(0.5 * this.timeliness_expert * (2 - this.Beta * t*t) , 0);
            }
        }

        protected override void CalculateCoefficients()
        {
            this.Gamma = this.alpha_expert > 1 ? 0 : 1+0.5/(this.alpha_expert*this.alpha_expert)*this.timeliness_expert;
            this.Alpha = this.alpha_expert > 1 ? 0 : 1 + 0.05 * this.alpha_expert * this.alpha_expert / (this.Gamma) * this.fullness_expert;
            //this.Beta = this.alpha_expert > 1 ? 0 : 1 + (this.alpha_expert/(this.Gamma*this.Gamma)*this.reliability_expert * 1e-2);
            this.Beta = this.alpha_expert > 1 ? 0 : this.alpha_expert / (this.Gamma * this.Gamma) * this.reliability_expert  *1e-3;
        }
    }

    class CustomCell : BaseCell
    {
        public CustomCell(double fe, double re, double te, double ae) : base(fe, re, te, ae) { }
        public override Func<double, double> Fullness
        {
            get
            {
                return (t) => Math.Min(0.8+this.alpha_expert * this.fullness_expert * Math.Pow(t, 2) / Math.Exp(2.5 * this.Gamma), 1);
                //return (t) => Math.Min(this.alpha_expert * this.fullness_expert * Math.Pow(1.5, t) / Math.Exp(9 * this.Gamma), 1);
            }
        }

        public override Func<double, double> Reliability
        {
            get
            {
                return (t) => Math.Min(1e-2*this.reliability_expert /20*Math.Pow(1+this.Beta, 2) * t*t, 1);
            }
        }

        public override Func<double, double> Timeliness
        {
            get
            {
                return (t) => Math.Max(1 - this.timeliness_expert * (this.alpha_expert / (Math.Pow(this.Gamma, 5)) * t + 0.5 * this.alpha_expert + 0.25 * this.Beta), 0);

                //return (t) => Math.Max(1 - this.Gamma * (this.timeliness_expert + this.fullness_expert) * (t / 4 + 0.5 * this.alpha_expert + 0.25 * this.Beta), 0);
                //return (t) => Math.Max(1-this.Gamma*(this.timeliness_expert+this.fullness_expert)*(t/4+0.5*this.alpha_expert+0.25*this.Beta), 0);
            }
        }

        protected override void CalculateCoefficients()
        {
            this.Alpha = this.alpha_expert > 1 ? 0 : Math.Exp(-this.fullness_expert) * Math.Pow(this.alpha_expert, 2) * this.fullness_expert / (1+this.reliability_expert*this.alpha_expert);
            this.Beta = this.alpha_expert > 1 ? 0 : this.timeliness_expert*this.fullness_expert*(1+this.reliability_expert+Math.Exp(2*this.alpha_expert))/(1e3+Math.Exp(10*this.timeliness_expert));
            this.Gamma = this.alpha_expert > 1 ? 0 : (1+Math.Exp(0.5*this.reliability_expert*this.alpha_expert))/(Math.Pow(this.fullness_expert, 0.5)+Math.Log(this.timeliness_expert+0.5*this.alpha_expert,2));
        }
    }
}