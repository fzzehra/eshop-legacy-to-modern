
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 5,
  duration: '30s',
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:50586';

export default function () {
  const response = http.get(`${BASE_URL}/Catalog/Details/1`);

  check(response, {
    'HTTP 200 dondu': (r) => r.status === 200,
    'Urun bilgisi gorundu': (r) => r.body.includes('.NET Bot Black Hoodie'),
  });

  sleep(1);
}
