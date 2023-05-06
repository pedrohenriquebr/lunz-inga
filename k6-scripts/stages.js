import http from "k6/http";
import { check, csv, sleep } from "k6";
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { SharedArray } from 'k6/data';




const csvData = new SharedArray('emails', function () {
  // Load CSV file and parse it using Papa Parse
  return papaparse.parse(open('./emails.csv'), { header: false }).data;
});

export let options = {
  thresholds: {
    http_req_duration: ["p(95)<200"],
  },
  stages: [
    { duration: "2m", target: 5 },
  ]
};

export default function () {
  const randomEmail = csvData[Math.floor(Math.random() * csvData.length)];
  const route = 'http://api:3000/api/newsletter-subscription/check-email/' + randomEmail;
  check(http.get(route), {
    "status is 200": (r) => r.status === 200,
  });

  sleep(1);
}
