$(function () {
  'use strict';
  if ($('#morris-line-example').length) {
    Morris.Line({
      element: 'morris-line-example',
      lineColors: ['#419B9C', '#F36368', '#76C1FA', '#FABA66'],
      data: [{
        y: '2018',
        a: 8858,
        b: 356
      },
      {
        y: '2019',
        a: 7888,
        b: 464
      },
      {
        y: '2020',
        a: 6255,
        b: 358
      },
      {
        y: '2021',
        a: 4168,
        b: 458
      },
      {
        y: '2022',
        a: 7855,
        b: 445
      },
      {
        y: '2023',
        a: 9855,
        b: 325
      },
      {
        y: '2024',
        a: 7522,
        b: 552
      }
      ],
      xkey: 'y',
      ykeys: ['a', 'b'],
      labels: ['Incoming Calls', 'Outgoing Calls'],
    });
  }

  if ($('#morris-area-example').length) {
    Morris.Area({
      element: 'morris-area-example',
      lineColors: ['#76C1FA', '#F36368', '#63CF72', '#FABA66'],
      data: [{
        y: '2006',
        a: 100,
        b: 90
      },
      {
        y: '2007',
        a: 75,
        b: 105
      },
      {
        y: '2008',
        a: 50,
        b: 40
      },
      {
        y: '2009',
        a: 75,
        b: 65
      },
      {
        y: '2010',
        a: 50,
        b: 40
      },
      {
        y: '2011',
        a: 75,
        b: 65
      },
      {
        y: '2012',
        a: 100,
        b: 90
      }
      ],
      xkey: 'y',
      ykeys: ['a', 'b'],
      labels: ['Series A', 'Series B']
    });
  }
  if ($("#morris-bar-example").length) {
    Morris.Bar({
      element: 'morris-bar-example',
      barColors: ['#419B9C', '#F36368', '#76C1FA', '#FABA66'],
      data: [{
        y: 'Assault',
        a: 100,
        b: 90
      },
      {
        y: 'Abuse',
        a: 75,
        b: 65
      },
      {
        y: 'Rape',
        a: 50,
        b: 40
      },
      {
        y: 'Drugs',
        a: 75,
        b: 65
      },
      {
        y: 'Suicidal',
        a: 50,
        b: 40
      },
      {
        y: 'Guns',
        a: 75,
        b: 65
      },
      {
        y: 'Other',
        a: 100,
        b: 90
      }
      ],
      xkey: 'y',
      ykeys: ['a', 'b'],
      labels: ['Male', 'Female']
    });
  }
  if ($("#morris-donut-example").length) {
    Morris.Donut({
      element: 'morris-donut-example',
      colors: ['#76C1FA', '#F36368', '#63CF72', '#FABA66'],
      data: [{
        label: "Download Sales",
        value: 12
      },
      {
        label: "In-Store Sales",
        value: 30
      },
      {
        label: "Mail-Order Sales",
        value: 20
      }
      ]
    });
  }

  if ($("#morris-donut-dark-example").length) {
    Morris.Donut({
      element: 'morris-donut-dark-example',
      colors: ['#76C1FA', '#F36368', '#63CF72', '#FABA66'],
      data: [{
        label: "Download Sales",
        value: 12
      },
      {
        label: "In-Store Sales",
        value: 30
      },
      {
        label: "Mail-Order Sales",
        value: 20
      }
      ],
      labelColor: "#b1b1b5"
    });
  }

  if ($('#morris-dashboard-taget').length) {
    Morris.Area({
      element: 'morris-dashboard-taget',
      parseTime: false,
      lineColors: ['#76C1FA', '#F36368', '#63CF72', '#FABA66'],
      data: [{
        y: 'Jan',
        Revenue: 190,
        Target: 170
      },
      {
        y: 'Feb',
        Revenue: 60,
        Target: 90
      },
      {
        y: 'March',
        Revenue: 100,
        Target: 120
      },
      {
        y: 'Apr',
        Revenue: 150,
        Target: 140
      },
      {
        y: 'May',
        Revenue: 130,
        Target: 170
      },
      {
        y: 'Jun',
        Revenue: 200,
        Target: 160
      },
      {
        y: 'Jul',
        Revenue: 150,
        Target: 180
      },
      {
        y: 'Aug',
        Revenue: 170,
        Target: 180
      },
      {
        y: 'Sep',
        Revenue: 140,
        Target: 90
      }
      ],
      xkey: 'y',
      ykeys: ['Target', 'Revenue'],
      labels: ['Monthly Target', 'Monthly Revenue'],
      hideHover: 'auto',
      behaveLikeLine: true,
      resize: true,
      axes: 'x'
    });
  }
});