using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CellularGUI.RuleManager
{
    public class RuleEditorCycle
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private NeighborhoodChooser neighborhoodChooser;

        private RuleEditor ruleEditor;

        public RuleEditor RuleEditor
        {
            get { return ruleEditor; }
            set { ruleEditor = value; }
        }


        private Grid editor;

        public Grid Editor
        {
            get { return editor; }
            set { editor = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public RuleEditorCycle(Grid editor)
        {
            Editor = editor;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void RunNeighborhoodChooser()
        {
            neighborhoodChooser = new NeighborhoodChooser(this);
        }

        public void RunRuleEditor(Rule rule)
        {
            ruleEditor = new RuleEditor(this, rule);
        }

        public void RunRuleEditor(NeighborhoodType neighborhoodType, RuleType ruleType)
        {
            ruleEditor = new RuleEditor(this, neighborhoodType, ruleType);
        }
    }
}
