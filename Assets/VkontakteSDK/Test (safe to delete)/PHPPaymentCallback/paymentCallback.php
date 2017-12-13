<?php
header("Content-Type: application/json; encoding=utf-8");

$secret_key = 'JG3wNNTAiRn8R1MXgRaB'; // App secret key

$input = $_POST;

// Checking signature
$sig = $input['sig'];
unset($input['sig']);
ksort($input);
$str = '';
foreach ($input as $k => $v) {
    $str .= $k.'='.$v;
}

if ($sig != md5($str.$secret_key)) {
    $response['error'] = array(
        'error_code' => 10,
        'error_msg' => 'Calculated and passed signatures are not the same',
        'critical' => true
    );
} else {
    // Correct signature
    switch ($input['notification_type']) {
        case 'get_item':
            // Getting info about the product
            $item = $input['item']; // Product name

            if ($item == 'item1') {
                $response['response'] = array(
                    'item_id' => 25,
                    'title' => '300 gold coins',
                    'photo_url' => 'http://somesite/images/coin.jpg',
                    'price' => 5
                );
            } elseif ($item == 'item2') {
                $response['response'] = array(
                    'item_id' => 27,
                    'title' => '500 gold coins',
                    'photo_url' => 'http://somesite/images/coin.jpg',
                    'price' => 10
                );
            } else {
                $response['error'] = array(
                    'error_code' => 20,
                    'error_msg' => 'Product is not exist.',
                    'critical' => true
                );
            }
            break;

        case 'get_item_test':
            // Getting info about the product in test mode
            $item = $input['item'];
            if ($item == 'item1') {
                $response['response'] = array(
                    'item_id' => 125,
                    'title' => '300 gold coins (test mode)',
                    'photo_url' => 'http://somesite/images/coin.jpg',
                    'price' => 5
                );
            } elseif ($item == 'item2') {
                $response['response'] = array(
                    'item_id' => 127,
                    'title' => '500 gold coins (test mode)',
                    'photo_url' => 'http://somesite/images/coin.jpg',
                    'price' => 10
                );
            } else {
                $response['error'] = array(
                    'error_code' => 20,
                    'error_msg' => 'Product is not exist.',
                    'critical' => true
                );
            }
            break;

        case 'order_status_change':
            // Changins order status
            if ($input['status'] == 'chargeable') {
                $order_id = intval($input['order_id']);

// Code of checking product, include its price
                $app_order_id = 1; // Order ID that you've got

                $response['response'] = array(
                    'order_id' => $order_id,
                    'app_order_id' => $app_order_id,
                );
            } else {
                $response['error'] = array(
                    'error_code' => 100,
                    'error_msg' => 'Incorrect chargeable.',
                    'critical' => true
                );
            }
            break;

        case 'order_status_change_test':
            // Changing order status in test mode
            if ($input['status'] == 'chargeable') {
                $order_id = intval($input['order_id']);

                $app_order_id = 1; // Here is no real order cause of test mode

                $response['response'] = array(
                    'order_id' => $order_id,
                    'app_order_id' => $app_order_id,
                );
            } else {
                $response['error'] = array(
                    'error_code' => 100,
                    'error_msg' => 'Incorrect chargeable.',
                    'critical' => true
                );
            }
            break;
    }
}

echo json_encode($response);
?> 