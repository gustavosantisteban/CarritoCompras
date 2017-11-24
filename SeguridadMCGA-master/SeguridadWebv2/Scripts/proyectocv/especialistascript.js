//$(function () {
//    $('#fechaconsulta').datetimepicker({
//        format: "dd-mm-yyyy",
//        daysOfWeekDisabled: [0, 6],
//        inline: true,
//        useCurrent: true,
//        sideBySide: false
//    });

//    var fecha = $("td.day.active").attr("data-day");
//    var idesp = $(".idesp").attr("value");
//    $.ajax({
//        type: "GET",
//        url: '@Url.Action("Turnos", "Especialista")',
//        dataType: "JSON",   
//        contentType: "application/json; charset=utf-8",
//        data: { id: idesp, dato: fecha },
//        success: function (data) { alert('Success'); },
//        error: function (xhr, err) {
//            alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
//            alert("responseText: " + xhr.responseText);
//        }
//    });

//        //$('#fechaconsulta day.active').change(function () {
//        //    alert("aca");
//        //    var fecha = $("td.day.active").attr("data-day");
//        //    $.ajax({
//        //        type: "GET",
//        //        url: '@Url.Action("SearchtoDateTurno", "Especialista")',
//        //        dataType: "JSON",
//        //        contentType: "application/json; charset=utf-8",
//        //        data: { dato: fecha },
//        //        success: function (data) { alert('Success'); },
//        //        error: function (xhr, err) {
//        //            alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
//        //            alert("responseText: " + xhr.responseText);
//        //        }
//        //    });
//        //});
    
//    //$('td.day.active').on('click', function () {
//    //    alert("aca");
//    //    var fecha = $("td.day.active").attr("data-day");
//    //    $.ajax({
//    //        type: "GET",
//    //        url: '@Url.Action("SearchtoDateTurno", "Especialista")',
//    //        dataType: "JSON",
//    //        contentType: "application/json; charset=utf-8",
//    //        data: { dato: fecha },
//    //        success: function (data) { alert('Success'); },
//    //        error: function (xhr, err) {
//    //            alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
//    //            alert("responseText: " + xhr.responseText);
//    //        }
//    //    });
//    //});
//});
