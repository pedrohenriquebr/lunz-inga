import http from "k6/http";
import { check, csv, sleep } from "k6";
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { SharedArray } from 'k6/data';




const csvData = new SharedArray('emails', function () {
    // Load CSV file and parse it using Papa Parse
    return papaparse.parse(open('./emails.csv'), {header: false}).data;
  });

export let options = {
  thresholds: {
    http_req_duration: ["p(95)<200"], // add threshold for response times below 200ms
  },
  stages: [
    { duration: "1m", target: 10 }, // Ramp up to 100 VUs over 10 minutes
    { duration: "2m", target: 10 }, // Hold at 100 VUs for 20 minutes
    { duration: "1m", target: 0 },   // Ramp down to 0 VUs over 10 minutes
  ]
};

export default function () {
    const LUZINGA_API = 'http://api:3000/api/newsletter-subscription/check-email/:email';
   const randomEmail = csvData[Math.floor(Math.random() * csvData.length)];
  check(http.get(LUZINGA_API.replace(':email', randomEmail)), {
    "status is 200": (r) => r.status === 200,
  });
}
