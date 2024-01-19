using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;

namespace TVAnime.Models
{
    internal class SelectionItem: INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Param { get; set; }

        private Color BackgroundColorValue { get; set; }
        public Color BackgroundColor 
        {
            get
            {
                return this.BackgroundColorValue;
            }
            set
            {
                if (value != BackgroundColorValue) 
                {
                    BackgroundColorValue = value;
                    NotifyPropertyChanged(nameof(BackgroundColor));
                }
            }
        }


        public SelectionItem(string name, string id, Dictionary<string, object> param = null)
        {
            this.Name = name;
            this.BackgroundColor = Color.White;
            this.Id = id;
            this.Param = param;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
