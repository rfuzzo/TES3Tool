﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.Services;

namespace Tes3EditX.Backend.ViewModels
{
    public partial class CompareViewModel : ObservableObject
    {
        public NotificationService? NotificationService;

        public CompareViewModel(INotificationService notificationService)
        {
            NotificationService = notificationService as NotificationService;
        }

      
    }
}
