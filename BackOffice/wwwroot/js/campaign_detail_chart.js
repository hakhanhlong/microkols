var chartData; // globar variable for hold chart data  
google.load("visualization", "1", { packages: ["corechart"] });
// Here We will fill chartData  

function load_campaign_detail_chart(campaignid) {
    $.ajax({
        url: "/ajaxchart/Statistic_JsonCampaignDetailRevenue",
        data: JSON.stringify({ "Id": campaignid }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            chartData = data;
        },
        error: function () {
            alert("Error loading data! Please try again.");
        }
    }).done(function () {
        // after complete loading data  
        google.setOnLoadCallback(drawChart);
        drawChart();
    });

}

function drawChart() {

    var data = google.visualization.arrayToDataTable(chartData);
    var options = {
        title: "Chi phí & doanh thu chiến dịch",
        width: 900,
        height: 560
    };

    var lineChart = new google.visualization.PieChart(document.getElementById('chart_div'));
    lineChart.draw(data, options);
} 