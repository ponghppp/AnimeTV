﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tizen.NUI;

namespace TVAnime.Models
{
    internal class SelectionItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public Dictionary<string, object> Param { get; set; }
        public int Percentage { get; set; }

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

        public SelectionItem(string name, Dictionary<string, object> param = null, int percentage = 0)
        {
            this.Name = name;
            this.BackgroundColor = Color.White;
            this.Param = param;
            this.Percentage = percentage;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
