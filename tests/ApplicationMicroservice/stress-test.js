import http from 'k6/http'
import {sleep} from 'k6'

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        {duration: '10s', target: 1000},
        {duration: '40', target: 1000},
        {duration: '10s', target: 0}
    ],
};

export default () => {
    for (let i = 0; i < 100; ++i)
        http.get(`http://localhost:5163/api/orders?page=${i}`)

    sleep(1)
}