import http from 'k6/http'
import {sleep} from 'k6'

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        {duration: '1m', target: 1000},
        {duration: '3m', target: 1000},
        {duration: '1m', target: 0}
    ],
    thresholds: {
        http_req_duration: ['p(99) < 150'],
    },
};

export default () => {
    for (let i = 0; i < 100; ++i)
        http.get(`http://localhost:5163/api/orders?page=${i}`)

    sleep(1)
}