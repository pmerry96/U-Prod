using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class Activity : object
{
    //Private:
    // FIELDS
    private string name;
    private string description;
    private DateTime start;
    private DateTime stop;
    private bool complete;
    //methods
    private void setName(string _name)
    {
        name = _name;
    }

    private void setDescription(string _description)
    {
        description = _description;
    }

    private void setStart(DateTime _start)
    {
        start = _start;
    }

    private void setStop(DateTime _stop)
    {
        stop = _stop;
    }

    private void setCompletion(bool _complete)
    {
        complete = _complete;
    }


    //Public:
    public string getName()
    {
        return name;
    }

    public string getDesc()
    {
        return description;
    }

    public DateTime getStartTime()
    {
        return start;
    }

    public DateTime getStopTime()
    {
        return stop;
    }

    public bool isComplete()
    {
        return complete;
    }

    public Activity(string _name, string _description, DateTime _start, DateTime _stop, bool _complete)
    {
        name = _name;
        description = _description;
        start = _start;
        if(start >= _stop)
        {
            _stop = start.AddHours(1);
        }
        stop = _stop;
        complete = _complete;
    }

    
    public override String ToString()
    {
        String theString;
                                                                                                        //Ternary operator to turn "True" to "Yes" and "False" to "No".
        theString = name + "Start: " + start.ToString() + " End: " + stop.ToString() + " Completed: " + (complete ? "Yes":"No");
        return theString;
    }

    public override bool Equals(object obj)
    {
        if(this.GetType().Equals(obj.GetType()))
        {
            Activity other = (Activity) obj;
            //return true IFF all fields contain identical values
            return (this.name.Equals(other.name) && this.start.Equals(other.start) && this.stop.Equals(other.stop) && this.complete.Equals(other.complete));
        }
        //objs are of diff types and therefore cannot be equal
        return false;
    }


};

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        //Model Objects
        List<Activity> activities;


        //Persistent widgets
        private Button profilebutton;
        private Button metricsbutton;
        private Button calendarbutton;
        private Button logoutbutton;

        //Calendar Screen widgets
        private ListBox CalendarActivitiesBox;
        private List<int> calendar_activities;
        private Button addactivitybutton;
        private Button deleteactivitybutton;
        private Label startpagelabel;

        //Metrics Screen Widgets
        private MonthCalendar startcalendar;
        private MonthCalendar endcalendar;
        private Label startdatelabel, enddatelabel, metricslabel;
        private ListBox metricsbox;



        //Profile Screen Widgets
        private Label thebox;

        //adding activities buttons
        Label addactivity_namelabel;
        TextBox addactivity_name_entry;
        Label addactivity_startlabel;
        DateTimePicker addactivity_starttime_entry;
        Label addactivity_stoplabel;
        DateTimePicker addactivity_stoptime_entry;
        Label addactivity_completedlabel;
        CheckBox addactivity_complete_entry;
        Button addactivity_confirmbutton;
        Button addactivity_cancelbutton;

        public Form1()
        {
            //Model Init
            activities = new List<Activity>();
            //GUI init
            InitializeComponent();
            this.Text = "U-Prod";
            this.Size = new Size(1280, 720);


            profilebutton = new Button();
            profilebutton.Text = "My Profile";
            profilebutton.Location = new Point(1, 1);
            profilebutton.Click += profilebutton_click;
            this.Controls.Add(profilebutton);

            metricsbutton = new Button();
            metricsbutton.Text = "Metrics";
            metricsbutton.Location = new Point(profilebutton.Left, profilebutton.Bottom + 1);
            metricsbutton.Click += metricsbutton_click; 
            this.Controls.Add(metricsbutton);

            calendarbutton = new Button();
            calendarbutton.Text = "Activity List";
            calendarbutton.Size = new Size(calendarbutton.Width, calendarbutton.Height);
            calendarbutton.Location = new Point(metricsbutton.Left, metricsbutton.Bottom + 1);
            calendarbutton.Click += new EventHandler(calendarbutton_click);
            this.Controls.Add(calendarbutton);

            logoutbutton = new Button();
            logoutbutton.Text = "Logout";
            logoutbutton.Location = new Point(profilebutton.Left, calendarbutton.Bottom + 1);
            logoutbutton.Click += new EventHandler(logoutbutton_click);
            this.Controls.Add(logoutbutton);
        }

        private void logoutbutton_click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void calendarbutton_click(object sender, System.EventArgs e)
        {
            Form1_Load(new object(), new EventArgs());

            CalendarActivitiesBox = new ListBox();
            CalendarActivitiesBox.Text = "Activites Done";
            CalendarActivitiesBox.Location = new Point(100, 20);
            CalendarActivitiesBox.Size = new Size((this.Width) - 200, this.Height - 114);
            /*
            calendar_activities = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                calendar_activities.Add(i);
            }
            */
            CalendarActivitiesBox.DataSource = /*calendar_*/activities;
            this.Controls.Add(CalendarActivitiesBox);

            addactivitybutton = new Button();
            addactivitybutton.Text = "Add Activity";
            addactivitybutton.Location = new Point(CalendarActivitiesBox.Left, CalendarActivitiesBox.Bottom+1);
            addactivitybutton.Click += addactivity_click;
            this.Controls.Add(addactivitybutton);

            deleteactivitybutton = new Button();
            deleteactivitybutton.Text = "Delete Activity";
            deleteactivitybutton.Location = new Point(addactivitybutton.Right + 1, CalendarActivitiesBox.Bottom+1);
            deleteactivitybutton.Click += delete_click;
            this.Controls.Add(deleteactivitybutton);

            startpagelabel = new Label();
            startpagelabel.Text = "My Activities";
            startpagelabel.Location = new Point(this.Width / 2, 1);
            startpagelabel.Font = new Font(startpagelabel.Font, FontStyle.Bold);
            this.Controls.Add(startpagelabel);
        }

        private void metricsbutton_click(object sender, System.EventArgs e)
        {
            Form1_Load(new object(), new EventArgs());


            startcalendar = new MonthCalendar();
            endcalendar = new MonthCalendar();
            startcalendar.Location = new Point(90, 120);
            endcalendar.Location = new Point(startcalendar.Left, startcalendar.Bottom + 10);
            //this.Controls.Add(startcalendar);
            //this.Controls.Add(endcalendar);
            /*
            startdatelabel = new Label();
            startdatelabel.Text = "Start Date";
            startdatelabel.Location = new Point(startcalendar.Left - 60, startcalendar.Top);
            this.Controls.Add(startdatelabel);

            enddatelabel = new Label();
            enddatelabel.Text = "End Date";
            enddatelabel.Location = new Point(endcalendar.Left - 60, endcalendar.Top);
            this.Controls.Add(enddatelabel);
            */
            metricslabel = new Label();
            metricslabel.Text = "Productivity Metrics";
            metricslabel.Location = new Point(this.Width / 2, 1);
            metricslabel.Font = new Font(metricslabel.Font, FontStyle.Bold);
            this.Controls.Add(metricslabel);

            metricsbox = new ListBox();
            metricsbox.Location = new Point(startcalendar.Right + 10, metricslabel.Bottom + 1);
            metricsbox.Size = new Size(this.Width - startcalendar.Right - 64, this.Height - 72);

            List<String> metricslist = new List<String>();
            metricslist.Add("Name\t|Portion Of Total Tracked Time");
            TimeSpan totalelapsed = new TimeSpan();
            foreach (Activity a in activities)
            {
                totalelapsed += a.getStopTime() - a.getStartTime();
            }
            foreach(Activity a in activities)
            {
                metricslist.Add(a.getName() + "\t" + (((a.getStopTime() - a.getStartTime()).TotalSeconds / (totalelapsed.TotalSeconds))*100).ToString() + "%");
            }
            metricsbox.DataSource = new List<String>();
            metricsbox.Refresh();
            metricsbox.DataSource = metricslist;
            metricsbox.Refresh();
            this.Controls.Add(metricsbox);
        }

        private void profilebutton_click(object sender, System.EventArgs e)
        {
            Form1_Load(new object(), new EventArgs());
            thebox = new Label();
            thebox.Text = "Placeholder for non-applicable page";
            thebox.Size = new Size(250, 500);
            thebox.Location = new Point(Width/2, Height/2);
            this.Controls.Add(thebox);
        }

        private void delete_click(object sender, System.EventArgs e)
        {
            //Add code for deleting an element
            //at the very worst you can get all the data of the box
            // and the naively redraw the entire box
            foreach(Activity a in CalendarActivitiesBox.SelectedItems.OfType<Activity>().ToList())
            {
                activities.Remove(a);
            }
            CalendarActivitiesBox.DataSource = new List<Activity>();
            CalendarActivitiesBox.Refresh();
            CalendarActivitiesBox.DataSource = activities;
            CalendarActivitiesBox.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            this.Controls.Clear();
            this.Text = "U-Prod";
            this.Size = new Size(1280, 720);

            this.Controls.Add(profilebutton);

            this.Controls.Add(metricsbutton);

            this.Controls.Add(calendarbutton);

            this.Controls.Add(logoutbutton);
        }

        private void addactivity_click(object sender, EventArgs e)
        {
            //Show a label for activity name entry
            addactivity_namelabel = new Label();
            addactivity_namelabel.Text = "Activity Name: ";
            addactivity_namelabel.Location = new Point(deleteactivitybutton.Right + 10, deleteactivitybutton.Top);
            this.Controls.Add(addactivity_namelabel);
            //show a text entry box for Name
            addactivity_name_entry = new TextBox();
            addactivity_name_entry.BorderStyle = BorderStyle.Fixed3D;
            addactivity_name_entry.ReadOnly = false; //allow user input
            addactivity_name_entry.Location = new Point(addactivity_namelabel.Right + 3, addactivity_namelabel.Top);
            this.Controls.Add(addactivity_name_entry);
            //show a name for start time entry box
            addactivity_startlabel = new Label();
            addactivity_startlabel.Text = "Start Time: ";
            addactivity_startlabel.Location = new Point(addactivity_name_entry.Right + 10, addactivity_name_entry.Top);
            this.Controls.Add(addactivity_startlabel);
            //show a time entry box for start
            addactivity_starttime_entry = new DateTimePicker();
            addactivity_starttime_entry.Location = new Point(addactivity_startlabel.Right + 3, addactivity_startlabel.Top);
            addactivity_starttime_entry.Format = DateTimePickerFormat.Time;
            this.Controls.Add(addactivity_starttime_entry);
            //show a name for stop time entry box
            addactivity_stoplabel = new Label();
            addactivity_stoplabel.Text = "Stop Time: ";
            addactivity_stoplabel.Location = new Point(addactivity_starttime_entry.Right + 10, addactivity_starttime_entry.Top);
            this.Controls.Add(addactivity_stoplabel);
            //show a time entry box for stop
            addactivity_stoptime_entry = new DateTimePicker();
            addactivity_stoptime_entry.Location = new Point(addactivity_stoplabel.Right + 3, addactivity_stoplabel.Top);
            addactivity_stoptime_entry.Format = DateTimePickerFormat.Time;
            this.Controls.Add(addactivity_stoptime_entry);
            //show completed checkbox name
            addactivity_completedlabel = new Label();
            addactivity_completedlabel.Text = "Complete? ";
            addactivity_completedlabel.Location = new Point(addactivity_stoptime_entry.Right + 10, addactivity_stoptime_entry.Top);
            this.Controls.Add(addactivity_completedlabel);
            //show check box for Completed
            addactivity_complete_entry = new CheckBox();
            addactivity_complete_entry.Location = new Point(addactivity_completedlabel.Right + 3, addactivity_completedlabel.Top);
            this.Controls.Add(addactivity_complete_entry);
            //show a confirm button
            addactivity_confirmbutton = new Button();
            addactivity_confirmbutton.Text = "Confirm";
            addactivity_confirmbutton.Location = new Point(addactivitybutton.Left, addactivitybutton.Bottom + 3);
            addactivity_confirmbutton.Click += addactivity_confirmbutton_click;
            this.Controls.Add(addactivity_confirmbutton);
            //show a cancel button
            addactivity_cancelbutton = new Button();
            addactivity_cancelbutton.Text = "Cancel";
            addactivity_cancelbutton.Location = new Point(deleteactivitybutton.Left, deleteactivitybutton.Bottom + 3);
            addactivity_cancelbutton.Click += addactivity_cancelbutton_click;
            this.Controls.Add(addactivity_cancelbutton);
        }

        private void addactivity_confirmbutton_click(object sender, EventArgs e)
        {
            //construct activity
            Activity toadd = new Activity(addactivity_name_entry.Text + "\t", "", addactivity_starttime_entry.Value.ToLocalTime(), addactivity_stoptime_entry.Value.ToLocalTime(), addactivity_complete_entry.Checked);
            //On button click, read boxes and assign values
            activities.Add(toadd);
            CalendarActivitiesBox.DataSource = new List<Activity>();
            CalendarActivitiesBox.Refresh();
            CalendarActivitiesBox.DataSource = activities;
            CalendarActivitiesBox.Refresh();
            /*
            CalendarActivitiesBox = null;
            CalendarActivitiesBox = new ListBox();
            CalendarActivitiesBox.DataSource = activities;
            */
        }

        private void addactivity_cancelbutton_click(object sender, EventArgs e)
        {
            //hide all boxes, delete contents of them.
            this.Controls.Remove(addactivity_namelabel);
            addactivity_name_entry.Clear();
            this.Controls.Remove(addactivity_name_entry);

            this.Controls.Remove(addactivity_startlabel);
            this.Controls.Remove(addactivity_starttime_entry);

            this.Controls.Remove(addactivity_stoplabel);
            this.Controls.Remove(addactivity_stoptime_entry);

            this.Controls.Remove(addactivity_completedlabel);
            this.Controls.Remove(addactivity_complete_entry);

            this.Controls.Remove(addactivity_confirmbutton);
            this.Controls.Remove(addactivity_cancelbutton);
        }
    }
}
