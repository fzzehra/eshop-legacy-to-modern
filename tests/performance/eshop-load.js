
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 5,
  duration: '30s',
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5232';

export default function () {
  const response = http.get(`${BASE_URL}/`);

  check(response, {
    'HTTP 200 dondu': (r) => r.status === 200,
  });

  sleep(1);
}
