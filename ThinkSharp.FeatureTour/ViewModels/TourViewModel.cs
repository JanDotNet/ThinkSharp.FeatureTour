using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ThinkSharp.FeatureTouring.ViewModels
{
    /// <summary>
    /// ViewModel behind the Popup.
    /// </summary>
    public class TourViewModel : ViewModelBase, IPlacementAware
    {
        private readonly ITourRun myTourRun;
        private readonly string myClose = TextLocalization.Close;
        private readonly string myNext = TextLocalization.Next;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="tourRun">
        /// The <see langword="sealed"/>
        /// ITourRun object to be used by the view model to interact with the tour.
        /// May not be null.
        /// </param>
        protected internal TourViewModel(ITourRun tourRun)
        {
            if (tourRun == null)
                throw new ArgumentNullException(nameof(tourRun));
            myTourRun = tourRun;
            CmdDoIt = new RelayCommand(o => myTourRun.DoIt(), o => myTourRun.CanDoIt());
            CmdClose = new RelayCommand(o => myTourRun.Close());
            CmdNext = new RelayCommand(o =>
            {
                if (ButtonText == myClose)
                    myTourRun.Close();
                else
                    myTourRun.NextStep(false);
            }, o => ButtonText == myClose || myTourRun.CanNextStep());
            ButtonText = myNext;
        }

        #region Content

        private object myContent;
        public object Content
        {
            get { return myContent; }
            set { SetValue("Content", ref myContent, value); }
        }

        #endregion
        
        #region Header

        private object myHeader;
        public object Header
        {
            get { return myHeader; }
            set { SetValue("Header", ref myHeader, value); }
        }

        #endregion

        #region Steps

        private string mySteps;
        [Obsolete("Will be removed in furture releases.")]
        public string Steps
        {
            get { return mySteps; }
            set { SetValue("Steps", ref mySteps, value); }
        }

        #endregion

        #region Placement

        private Placement myPlacement;
        public Placement Placement
        {
            get { return myPlacement; }
            set
            {
                if (SetValue("Placement", ref myPlacement, value))
                    ActualPlacement = value;
            }
        }

        #endregion

        #region ActualPlacement

        private Placement myActualPlacement;
        public Placement ActualPlacement
        {
            get { return myActualPlacement; }
            set { SetValue("ActualPlacement", ref myActualPlacement, value); }
        }

        #endregion

        #region ContentTemplate

        private DataTemplate myContentTemplate;
        public DataTemplate ContentTemplate
        {
            get { return myContentTemplate; }
            set { SetValue("ContentTemplate", ref myContentTemplate, value); }
        }

        #endregion

        #region ContentTemplate

        private DataTemplate myHeaderTemplate;
        public DataTemplate HeaderTemplate
        {
            get { return myHeaderTemplate; }
            set { SetValue("HeaderTemplate", ref myHeaderTemplate, value); }
        }

        #endregion


        #region ButtonText

        private string myButtonText;
        public string ButtonText
        {
            get { return myButtonText; }
            set { SetValue("ButtonText", ref myButtonText, value); }
        }

        internal void SetCloseText()
        {
            ButtonText = myClose;
        }

        #endregion


        #region ShowDoIt

        private bool myShowDoIt;
        public bool ShowDoIt
        {
            get { return myShowDoIt; }
            set { SetValue("ShowDoIt", ref myShowDoIt, value); }
        }

        #endregion

        #region ShowNext

        private bool myShowNext;
        public bool ShowNext
        {
            get { return myShowNext || ButtonText == myClose; }
            set { SetValue("ShowNext", ref myShowNext, value); }
        }

        #endregion

        #region CurrentStepNo

        private int myCurrentStepNo = 1;
        /// <summary>
        /// Gets or sets the current step number (1-based) of the current tour.
        /// </summary>
        public int CurrentStepNo
        {
            get { return myCurrentStepNo; }
            internal set
            {
                if (SetValue("CurrentStepNo", ref myCurrentStepNo, value))
                    HasTourFinished = CurrentStepNo == TotalStepsCount;
            }
        }

        #endregion

        #region TotalStepCount

        private int myTotalStepsCount = 1;
        /// <summary>
        /// Gets or sets the number of total steps of the current tour.
        /// </summary>
        public int TotalStepsCount
        {
            get { return myTotalStepsCount; }
            internal set { SetValue("TotalStepsCount", ref myTotalStepsCount, value); }
        }

        #endregion

        #region HasTourFinished

        private bool myHasTourFinished;
        /// <summary>
        /// Gets a value that indicates if the current step is the last one.
        /// </summary>
        public bool HasTourFinished
        {
            get { return myHasTourFinished; }
            internal set { SetValue("HasTourFinished", ref myHasTourFinished, value); }
        }

        #endregion

        public ICommand CmdClose { get; }
        public ICommand CmdNext { get; }
        public ICommand CmdDoIt { get; }
    }
}
