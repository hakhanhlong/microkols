var chartData; // globar variable for hold chart data  
google.load("visualization", "1", { packages: ["corechart"] });
// Here We will fill chartData  


$(document).ready(function () {

    load_campaign_revenue_chart();
    $('#btn-statistic').click(function () {
        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();        
        load_campaign_revenue_chart(startDate, endDate);
    });
});


function load_campaign_revenue_chart(startDate, endDate) {
    $.ajax({
        url: "/ajaxchart/Statistic_JsonCampaignRevenue",
        data: JSON.stringify({ "startDate": startDate, "endDate": endDate }),        
        type: "POST",
        dataType: "json",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            chartData = data.data;

            $('.TotalOverAll_CampaignService').html(data.totalOvertallCampaignServiceCharge);
            $('.TotalOverAll_CampaignServicePayback').html(data.totalOverallCampaignServiceCashback);
            $('.TotalOverAll_CampaignAccountPayback').html(data.totalOverallampaignAccountPayback);
            $('.TotalOverAll_CampaignRevenue').html(data.totalOverallCampaignRevenue);
        },
        error: function () {
            alert("Error loading data! Please try again.");
        }
    }).done(function () {
        // after complete loading data  
        google.setOnLoadCallback(drawChartRevenue);
        drawChartRevenue();
    });

}

function drawChartRevenue() {

    var data = google.visualization.arrayToDataTable(chartData);
    var options = {
        title: "Chi phí & doanh thu chiến dịch",
        width: 900,
        height: 560
    };

    var pieChart = new google.visualization.PieChart(document.getElementById('chart_div'));
    pieChart.draw(data, options);
} 