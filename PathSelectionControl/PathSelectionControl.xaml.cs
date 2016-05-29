using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using WinForms = System.Windows.Forms;
using System.IO;

namespace UserControlLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PathSelectionControl : UserControl,IDataErrorInfo,INotifyPropertyChanged
    {
            
        public PathSelectionControl()
        {
            InitializeComponent();
                     
        }
        
        public String PathName
        {
            get { return (String)GetValue(PathNameProperty); }
            set
            { 
               
                 SetValue(PathNameProperty, value);
               
            }
        }
        // Using a DependencyProperty as the backing store for  TempGBDirectoryProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathNameProperty =
            DependencyProperty.Register("PathName", typeof(String), typeof(PathSelectionControl));//, new PropertyMetadata(null, new PropertyChangedCallback(OnPathChanged)));
      
        private void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

      

        public Boolean IsErrorInPath
        {

            get
            {
                return (Boolean)GetValue(IsErrorProperty);
            }

            set
            {
                SetValue(IsErrorProperty, value);
            }
        }

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register("IsErrorInPath", typeof(Boolean), typeof(PathSelectionControl));

        public String OtherPathName
        {
            get { return (String)GetValue(OtherPathNameProperty); }
            set
            {
                SetValue(OtherPathNameProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for  TempGBDirectoryProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OtherPathNameProperty =
            DependencyProperty.Register("OtherPathName", typeof(String), typeof(PathSelectionControl));//, new PropertyMetadata (null,new PropertyChangedCallback(OnPathChanged)));


        public static  void OnPathChanged(DependencyObject depo, DependencyPropertyChangedEventArgs e)
        {
             depo.SetValue(e.Property,e.NewValue);
        }

    

        private String lb; 
        public String LabelContent
        {
            get
            {
                return lb;
            }
            set
            {
                if (value != lb)
                {

                    lb = value;
                    OnPropertyChanged("LabelContent");
                }
             
            }
        }

       // private String warning;
        public String PathWarning
        {
            get
            {
                return (String)GetValue(WarningProperty); 
            }
            set
            {
                SetValue(WarningProperty, value);
            }
        }
        public static readonly DependencyProperty WarningProperty =
            DependencyProperty.Register("PathWarning", typeof(String), typeof(PathSelectionControl), new PropertyMetadata(null, new PropertyChangedCallback(OnWarningChanged)));

        private static void OnWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(WarningProperty, e.NewValue);
        }


        public event PropertyChangedEventHandler PropertyChanged;
      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            String savePath = String.Empty;
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDialog.SelectedPath = "C:\\";
            WinForms.DialogResult dialogResult = folderDialog.ShowDialog();
            if (dialogResult == WinForms.DialogResult.OK)
            {
                PathName = folderDialog.SelectedPath;
            }
         
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

     
    
        public string this[string propertyName]
        {
            get
            {
                String error = null;
                
               
                if (null != PathName)
                {
                  
                    if (!string.IsNullOrEmpty(PathName))
                    {

                        if (Directory.Exists(PathName))
                            {

                               //if (PathName.Equals(OtherPathName))
                               // {
                                   
                               //   //  error = "Server and Temporary directory cannot be same.";

                               // }
                               // else
                               // {
                                    try
                                    {

                                        if (new DriveInfo(PathName).AvailableFreeSpace < 8192000)
                                        {
                                            error = "No free space in  drive. Select a drive with more than 8MB of free space";

                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    FileStream fs = null;
                                    try
                                    {
                                        fs = new FileStream(System.IO.Path.Combine(PathName, "abcd.txt"), FileMode.CreateNew, FileAccess.ReadWrite);

                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        error = "No write access to path";
                                        if (null != fs)
                                        {

                                            fs.Close();
                                        }
                                    }
                                    catch (Exception)
                                    {


                                    }
                                    finally
                                    {
                                        if (null != fs)
                                        {
                                            fs.Close();
                                        }
                                        if (File.Exists(System.IO.Path.Combine(PathName, "abcd.txt")))
                                        {
                                            File.Delete(System.IO.Path.Combine(PathName, "abcd.txt"));
                                        }


                                    }

                              //  }
                            }
                            else
                            {
                                error = "Selected Path does not exist";

                            }

                        }
                    

                }
                PathWarning = error; 
                IsErrorInPath=!String.IsNullOrEmpty(error);
                return error;

            }
        }
               
    }

  
}