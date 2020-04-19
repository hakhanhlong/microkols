var chartData; // globar variable for hold chart data  
google.load("visualization", "1", { packages: ["corechart"] });
// Here We will fill chartData  
$(document).ready(function () {
    
    load_campaign_chart();
    $('#btn-statistic').click(function () {


        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();

        console.log(startDate, endDate);

        load_campaign_chart(startDate, endDate);

    });

    
});


function load_campaign_chart(startDate, endDate) {
    $.ajax({
        url: "/ajaxchart/Statistic_JsonCampaignPaid",
        data: JSON.stringify({ "startDate": startDate, "endDate": endDate}),        
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
        title: "Chi phí chiến dịch",
        pointSize: 5,
        legend: { position: 'bottom' }     
    };

    var lineChart = new google.visualization.LineChart(document.getElementById('chart_div'));
    lineChart.draw(data, options);
} 