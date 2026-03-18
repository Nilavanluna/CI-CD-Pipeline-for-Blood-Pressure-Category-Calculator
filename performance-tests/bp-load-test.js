import http from 'k6/http'
import { check, sleep } from 'k6'

export const options = {
  stages: [
    { duration: '30s', target: 10 },
    { duration: '1m', target: 10 },
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<3000'],
    http_req_failed: ['rate<0.05'],
  },
}

export default function () {
  const BASE_URL =
    __ENV.APP_URL || 'https://bp-staging-nilavan.azurewebsites.net'

  // Test GET request - load the page
  const getRes = http.get(BASE_URL)
  check(getRes, {
    'GET status 200': (r) => r.status === 200,
  })

  sleep(1)

  // Test POST request - submit a BP reading
  const postRes = http.post(
    BASE_URL,
    {
      'BP.Systolic': '120',
      'BP.Diastolic': '80',
    },
    {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    },
  )
  check(postRes, {
    'POST status 200': (r) => r.status === 200,
    'contains category': (r) => r.body.includes('Blood Pressure'),
  })

  sleep(1)
}
