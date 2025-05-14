using ST10122162_Agri_Energy_Connect_Platform.Models;

namespace ST10122162_Agri_Energy_Connect_Platform.ViewModels
{
    public class DashboardViewModel
    {
        // Farmer-specific properties
        public Farmer? Farmer { get; set; }
        
        // Employee-specific properties
        public int TotalFarmers { get; set; }
        public List<Farmer>? RecentFarmers { get; set; }
        
        // Common properties
        public int TotalProducts { get; set; }
        public List<Product>? RecentProducts { get; set; }
    }
}
