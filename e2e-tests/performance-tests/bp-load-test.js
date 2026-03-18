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
    __ENV.APP_URL ||
    'https://bp-staging-nilavan-epaebkbed7gah2ac.switzerlandnorth-01.azurewebsites.net'

  const getRes = http.get(BASE_URL)
  check(getRes, {
    'GET status 200': (r) => r.status === 200,
    'page contains BP': (r) => r.body.includes('BP'),
  })

  sleep(1)
}
