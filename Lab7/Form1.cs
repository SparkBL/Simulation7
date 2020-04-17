using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lab7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Spa spa; int month = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            message.Text = "";
            Environment.Reset((int)population.Value,(double)base_cost.Value,(double)base_salary.Value,(int)eq_state.Value);
            spa = new Spa((int)start_square.Value, (int)start_service.Value, (int)start_staff.Value, (int)start_equipment.Value);
            month = 1;

            clients.ChartAreas[0].AxisX.Minimum =0;
            clients.ChartAreas[0].AxisX.Maximum = 12;
            qual_capacity.ChartAreas[0].AxisX.Minimum = 0;
            qual_capacity.ChartAreas[0].AxisX.Maximum =12;
            cost_price.ChartAreas[0].AxisX.Minimum = 0;
            cost_price.ChartAreas[0].AxisX.Maximum = 12;
            staff_equip.ChartAreas[0].AxisX.Minimum = 0;
            staff_equip.ChartAreas[0].AxisX.Maximum = 12;
            profit_chart.ChartAreas[0].AxisX.Minimum = 0;
            profit_chart.ChartAreas[0].AxisX.Maximum = 12;



            period.Text = Environment.period.ToString();
            clients_income.Text = Environment.customers_income.ToString();  
            served.Text = spa.served.ToString();   
            quality.Text = spa.service_quality.ToString();    
            capacity.Text = spa.service_capacity.ToString();    
            capacity.Text = spa.service_capacity.ToString();    
            cost.Text = Service.cost.ToString();    
            equipment.Text = spa.equipment.Count.ToString();    
            staff.Text = spa.staff.Count.ToString();    

            clients.Series.Clear();
            clients.Series.Add("income");
            clients.Series.Add("served");
            clients.Series[0].Points.AddXY(month, Environment.customers_income);
            clients.Series[1].Points.AddXY(month, spa.served);

            qual_capacity.Series.Clear();
            qual_capacity.Series.Add("quality");
            qual_capacity.Series.Add("capacity");
            qual_capacity.Series[0].Points.AddXY(month, spa.service_quality);
            qual_capacity.Series[1].Points.AddXY(month, spa.service_capacity);

            cost_price.Series.Clear();
            cost_price.Series.Add("cost");
            cost_price.Series.Add("price");
            cost_price.Series[0].Points.AddXY(month, Service.cost);
            cost_price.Series[1].Points.AddXY(month, spa.GetAvgServicePrice());

            staff_equip.Series.Clear();
            staff_equip.Series.Add("staff");
            staff_equip.Series.Add("equipment");
            staff_equip.Series[0].Points.AddXY(month, spa.staff.Count);
            staff_equip.Series[1].Points.AddXY(month, spa.equipment.Count);

            profit_chart.Series[0].Points.Clear();
            profit_chart.Series[0].Points.AddXY(month, spa.profit);

            profit_month.Series[0].Points.Clear();

            timer1.Enabled = true;
            pause.Enabled = true;
            skip_month.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Environment.SendPotentialCustomer(spa);
            if (Environment.period % 240 == 0)
            {
                month++;
                profit_chart.Series[0].Points.AddXY(month, spa.profit);
                qual_capacity.Series[0].Points.AddXY(month, spa.service_quality);
                qual_capacity.Series[1].Points.AddXY(month, spa.service_capacity);
                cost_price.Series[0].Points.AddXY(month, Service.cost);
                cost_price.Series[1].Points.AddXY(month, spa.GetAvgServicePrice());
                profit_month.Series[0].Points.Clear();
               
                if (!spa.NextMonth()) { stop(); return; }
                
                {
                    
                }
                 if( month  >12)
                 {
                     clients.ChartAreas[0].AxisX.Minimum = month-8;
                     clients.ChartAreas[0].AxisX.Maximum = month+4;
                     qual_capacity.ChartAreas[0].AxisX.Minimum = month - 8;
                    qual_capacity.ChartAreas[0].AxisX.Maximum = month + 4;
                    cost_price.ChartAreas[0].AxisX.Minimum = month-8;
                     cost_price.ChartAreas[0].AxisX.Maximum = month + 4;
                    staff_equip.ChartAreas[0].AxisX.Minimum = month - 8;
                    staff_equip.ChartAreas[0].AxisX.Maximum = month + 4;
                    profit_chart.ChartAreas[0].AxisX.Minimum = month - 8;
                    profit_chart.ChartAreas[0].AxisX.Maximum = month + 4;

                }
            }
            else
            {
                if (!spa.NextHour()) { stop(); return; }
            }

            clients.Series[0].Points.AddXY(month, Environment.customers_income);
            clients.Series[1].Points.AddXY(month, spa.served);


            staff_equip.Series[0].Points.AddXY(month, spa.staff.Count);
            staff_equip.Series[1].Points.AddXY(month, spa.equipment.Count);

            profit_month.Series[0].Points.AddXY((int)(Environment.period / 8), spa.profit);
            

            period.Text = Environment.period.ToString();
            clients_income.Text = Environment.customers_income.ToString();
            served.Text = spa.served.ToString();
            quality.Text = Math.Round(spa.service_quality, 2).ToString();
            capacity.Text = Math.Round(spa.service_capacity, 2).ToString();
            cost.Text = Math.Round(Service.cost, 2).ToString();
            equipment.Text = spa.equipment.Count.ToString();
            staff.Text = spa.staff.Count.ToString();
            profit.Text = Math.Round(spa.profit, 2).ToString();
            price.Text = Math.Round(spa.GetAvgServicePrice(), 2).ToString();
        }

        private void stop()
        {
            timer1.Enabled = false;
            skip_month.Enabled = false;
            pause.Enabled = false;
            message.Text = "Spa has no staff or equipment anymore.\n It can't afford hiring/buying new ones.";
        }

        private void skip_month_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 240; i++) timer1_Tick(this, null);
        }

        private void speed_up_Click(object sender, EventArgs e)
        {
            if(timer1.Interval>=60)timer1.Interval -= 50;
        }

        private void speed_down_Click(object sender, EventArgs e)
        {
            timer1.Interval += 50;
        }

        private void pause_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled) { pause.Text = "Unpause"; timer1.Enabled = false; }
            else { pause.Text = "Pause"; timer1.Enabled = true; }
        }
    }



















    static class Environment
    {
        public static int predicted_customers_income = 2;
        public static int average_staff_search_time = 40;

        public static int population = 10000;
        public static int period = 0;
        public static int customers_income;
        public static double base_salary = 12000;
        public static double base_service_time = 3;
        public static double base_eq_cost = 30000;
        public static int base_eq_state= 150;
        public static Random rand = new Random();
        public static void SendPotentialCustomer(Spa s)
        {
            Customer n;
            int limit = (int) (Math.Log(population) * 0.7 * s.service_capacity / Math.Log(s.GetAvgServicePrice() ))+1;
        
                for (int i = 0; i < limit + rand.Next(0, limit / 5); i++)
                {
                    n = new Customer();
                    n.BuyService(s);
                    customers_income++;
                }
            
            period++;
        }

        public static void Reset(int _population,double _base_eq_cost, double _base_salary, int _base_eq_state)
        {
            period = 0;
            population = _population;
            customers_income = 0;
            base_eq_cost = _base_eq_cost;
            base_salary = _base_salary;
            base_eq_state = _base_eq_state;
        }

    }

    class Spa
    {
        public int served=0;
        public double service_capacity;
        private double salary_fund=0;
        private int square;
        public double service_quality;
        public double profit=0;
        public int service_count;
        private List<Service> services = new List<Service>();
        public List<Staff> staff = new List<Staff>();
        public List<Equipment> equipment = new List<Equipment>();
        public List<Equipment> reserved = new List<Equipment>();
        public List<int> vacancies = new List<int>();


        public Spa(int _square, int _services, int _staff, int _equipment)
        {
            if (_staff > _equipment) _staff=_equipment;
            if (_square < _equipment)  _square = _equipment;
            
                for (int i = 0; i < _services; i++) services.Add(new Service(Environment.base_service_time + Environment.rand.Next(-2, 1)));
                for (int i = 0; i < _staff; i++) staff.Add(new Staff(Environment.base_salary * 2, 1));
            for (int i = 0; i < _equipment; i++) { equipment.Add(new Equipment(Environment.base_eq_cost + Environment.rand.Next(-5000, 10000))); profit -= equipment[i].init_cost; }
                foreach (Staff st in staff) salary_fund += st.salary;
                service_count = _services;
                square = _square -equipment.Count;  
              
              
                UpdateServiceQuality();
                UpdateServiceCapacity();
                UpdateServiceCost();
            double p =  service_quality * (Environment.predicted_customers_income / service_capacity); 
                foreach (Service s in services) s.ChangePrice((1 + Environment.rand.Next(0,100)/100) * p);
            
        }
        private void UpdateServiceCost()   
        {
            double total_eq_cost = 0;
            int total_state = 0;
            foreach (Equipment eq in equipment) {
                total_eq_cost += eq.init_cost;
                total_state += eq.state;
            }
            Service.cost = ((double)salary_fund / (double)staff.Count) / (service_capacity * 30*8  / (double)staff.Count) + (((double)total_eq_cost / (double)equipment.Count) / ((double)total_state / (double)equipment.Count)); /////ADDED FORMULA
 
        }

        private void UpdateServiceQuality()
        {
            int total_state = 0;
            foreach (Equipment eq in equipment)
            {
                total_state += eq.state;
            }
            int total_qual=0;
            foreach (Staff st in staff) total_qual += st.qual;
            service_quality =( ((double)total_qual / (double)staff.Count) * ((double)total_state / (double)equipment.Count))/100.00;
        }

        private void UpdateServicePrice()
        {
            double p =  service_quality *( 0.1*((double)Environment.customers_income/((double)Environment.period)) / service_capacity);  
            foreach (Service s in services) s.ChangePrice(1 +( Environment.rand.Next(10,25))*0.1 * p);
        }


        private void UpdateServiceCapacity()  
        {
            int qual = 0;
            int states = 0;
            foreach (Staff st in staff) qual += (int)st.qual;
            foreach (Equipment eq in equipment) states += eq.state;
            service_capacity =( ((double)qual + (double)staff.Count)*(double)states/((double)equipment.Count)) / 8.00/100.00;
        }


        public void RenderService(int service) 
        {
            Service s = services[service];
            foreach ( Staff st in this.staff)
            {
                if (st.current_service == null)
                {
                    st.Work(s.Clone());
                    break;
                }
            }
            
        }
        public void CheckIfDone()
        {
            foreach (Staff st in this.staff) if (st.current_service!=null && st.current_service.time_needed <= 0)
                {
                    this.profit += (st.current_service.price - Service.cost);
                    st.current_service = null;
                    this.served++;
                 if(equipment.Count>0)  equipment[Environment.rand.Next(0, equipment.Count - 1)].state -= 1;
                }
            for (int i = 0;i < equipment.Count;i++) if (equipment[i].state <= 0)
                {
                    square++;
                    equipment.Remove(equipment[i]);
                    BuyEquipment();
                }
        }

        public bool NextHour()
        {
            foreach (Staff st in this.staff) if(st.current_service!=null) st.current_service.time_needed-=this.service_capacity;
            CheckIfDone();
            if(vacancies.Count==0)
                if (reserved.Count != 0)
                {
                    for (int i = 0; i < reserved.Count; i++)
                    {
                        equipment.Add(reserved[0]);
                        reserved.RemoveAt(0);
                    }
                }
       
            for (int i = 0; i < vacancies.Count;i++)
            {
                vacancies[i]--;
                if (vacancies[i] == 0)
                {
                    if (reserved.Count != 0)
                    {
                        equipment.Add(reserved[0]);
                        reserved.RemoveAt(0);
                    }
                    HireStaff();
                    vacancies.RemoveAt(i);
                }
                }
            if (equipment.Count == 0 || staff.Count == 0) return false; else return true;
        }

        public bool NextMonth()
        {
            BuyEquipment(true);
            int i = Environment.rand.Next(0, staff.Count * 10);
            if (i < staff.Count) { salary_fund -= staff[i].salary; staff.RemoveAt(i);  }
            for (int j=0; j<equipment.Count+reserved.Count-staff.Count;j++) vacancies.Add(Environment.average_staff_search_time + Environment.rand.Next(-20, 100));
            UpdateServiceCost();
            UpdateServiceQuality();
            UpdateServicePrice();
            UpdateServiceCapacity();
            Environment.period = 0;
           Environment.customers_income = 0;
            served = 0;
            foreach (Staff st in staff) st.current_service = null;
           return NextHour();
           
        }


        public void HireStaff( ) {
            int qual = Environment.rand.Next(0, 2);
            Staff n = new Staff(Environment.base_salary * (1 + qual),qual); 
            this.staff.Add(n);
            this.salary_fund += n.salary;
            UpdateServiceCost();
            UpdateServiceCapacity();
        }
        public void BuyEquipment(bool reserved = false) 
        {
            while (square > 0)
                if (profit / Environment.base_eq_cost >= 1)
                {
                    Equipment n = new Equipment(Environment.base_eq_cost + Environment.rand.Next(-1000, 2000));
                    if (reserved) this.reserved.Add(n);
                    else
                        this.equipment.Add(n);
                    square--;
                    this.profit -= n.init_cost;
                }
                else break;
        }

        public double GetAvgServicePrice()
        {
            double ret = 0;
            foreach(Service s in services) { ret += s.price; }
            return ret / services.Count;
        }
        public double GetSumServicePrice()
        {
            double ret = 0;
            foreach (Service s in services) { ret += s.price; }
            return ret;
        }
    }
 
    class Customer
    {
        public Customer() {
          
   
        }

        public void BuyService(Spa s) 
        {
           
            s.RenderService(Environment.rand.Next(0, s.service_count));
        }
    }
    class Staff
    {
        public int qual;
        public double salary;
        public Service current_service = null;
        public Staff(double _salary, int _qual)
        {
            this.salary = _salary;
            this.qual = _qual;
        }

        public void Work(Service _service)
        {
            this.current_service = _service;
        }
    }

    class Service
    {
        
        public double price;
      static  public double cost;
        public double time_needed;
        public Service(double _time_needed) { time_needed = _time_needed;  }
        public void ChangePrice(double sum) { price =cost* sum ; }
    
        public Service Clone()
        {
            Service clone = new Service(this.time_needed);
            clone.price = this.price;
            return clone;
        }
    }
    class Equipment
    {
        public int state = Environment.base_eq_state;
        public double init_cost;
        public Equipment(double c, bool _reserved = false) { this.init_cost = c;  }
    }
}
