(function () {
	'use strict';

	var myApp = angular.module('myApp', [
            "ngRoute" // <-- This is needed to use AngularJS Routing!  
	]);
	myApp.controller('DetailsController', function ($scope, $http, $routeParams, $location) { // <-- This is needed to handle extract/read parameters out of url
		$scope.params = $routeParams;
		 
		$http.get("/Cars/detail/" + $scope.params.Id)
            .then(function (response) {
            	$scope.car = response.data;
            });

	});
	




		myApp.controller('myCtrl', function ($scope, $http) {
		
			$http.get("/Cars/Getdatalist")
			   .then(function (response) {
			   	$scope.carlist = response.data;
			   });
		});
		myApp.config(function ($routeProvider) {
			$routeProvider
				 
				.when('/car/:Id', {
					templateUrl: '/Content/app/Details.html',
					controller: 'DetailsController'
				})
			  .when('/Index', {
			  	templateUrl: '/Content/app/mainlist.html',
			  	controller: 'myCtrl'
			  })

			.otherwise({
				redirectTo: '/Index'

			});

		});
	})();