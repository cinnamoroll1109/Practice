package com.example.bmr_calculator;

import androidx.appcompat.app.AppCompatActivity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.*;
import android.util.Log;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.protocol.HTTP;

import java.text.NumberFormat;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

//import org.apache.http.client.HttpClient;

public class Page3_output extends AppCompatActivity {
    //宣告物件
    private TextView tv_name;
    private TextView tv_bmi;
    private TextView tv_bmr;
    private Button btn_return;

    String i_or_u;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_page3_output);

        tv_name = findViewById(R.id.textView4);
        tv_bmi = findViewById(R.id.textView5);
        tv_bmr = findViewById(R.id.textView6);
        btn_return = findViewById(R.id.p3_btnReturn);

        String id = "";
        Bundle bundle = getIntent().getExtras();
        i_or_u = bundle.getString("insert_or_update");
        if(i_or_u.equals("update")){
            id = bundle.getString("id");
        }
        String name = bundle.getString("name");
        String sex = bundle.getString("sex");
        String age = bundle.getString("age");
        String height = bundle.getString("height");
        String weight = bundle.getString("weight");


        float f_age = Float.parseFloat(age);
        float f_h = Float.parseFloat(height);
        float f_w = Float.parseFloat(weight);
        float f_bmi, f_bmr;

        tv_name.setText(name);

        f_bmi = BMI(f_h, f_w);
        //取到小數點後二位
        NumberFormat nf = NumberFormat.getInstance();
        nf.setMaximumFractionDigits(2);
        tv_bmi.setText(nf.format(f_bmi));

        f_bmr = BMR(sex, f_age, f_h, f_w);
        tv_bmr.setText(nf.format(f_bmr));

        if(i_or_u.equals("insert")){
            insert_to_database(name, sex, age, height, weight, tv_bmr.getText().toString());
            Toast.makeText(this, "insert successfully", Toast.LENGTH_LONG).show();
        }
        else { //update
            update_to_database(id, name, sex, age, height, weight, tv_bmr.getText().toString());
            Toast.makeText(this, "update successfully", Toast.LENGTH_LONG).show();
        }


        View.OnClickListener btn_returnOnClick = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //切換頁面
                Intent it = new Intent();
                if(i_or_u .equals("insert")){
                    it.setClass(Page3_output.this, Page2_calculator.class);
                }
                else{
                    it.setClass(Page3_output.this, MainActivity.class);
                }
                startActivity(it);
            }
        };
        btn_return.setOnClickListener(btn_returnOnClick);
    }

    private float BMI(float h, float w){
        h = h / 100;
        h = h * h;

        return w / h;
    }

    private float BMR(String sex, float age, float h, float w){
        float result;
        if (sex == "man"){
            result = 13.7f * w + 5.0f * h - 6.8f * age + 66.0f;
        }
        else{
            result = 9.6f * w + 1.8f * h - 4.7f * age +655.0f;
        }

        return result;
    }

    private void thread_test(){
        Runnable testThread = new Runnable() {
            @Override
            public void run() {
                while(true){
                    Log.e("testThread:", "thread is working");
                }
            }
        };
        Thread t = new Thread(testThread);
        t.start();
    }

    private void insert_to_database(final String name, final String sex, final String age, final String height, final String weight, final String bmr){
        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                String url = "http://10.0.60.21/android_use/insert_to_mysql.php";
                //String url = "http://192.168.66.18/android_use/insert_to_mysql.php";
                //use post method
                HttpPost httpPost = new HttpPost(url);
                HttpResponse httpResponse;
                List<NameValuePair> var = new ArrayList<NameValuePair>();
                var.add(new BasicNameValuePair("name", name));
                var.add(new BasicNameValuePair("sex", sex));
                var.add(new BasicNameValuePair("age", age));
                var.add(new BasicNameValuePair("height", height));
                var.add(new BasicNameValuePair("weight", weight));
                var.add(new BasicNameValuePair("bmr", bmr));
                try{
                    UrlEncodedFormEntity urlEncodedFormEntity = new UrlEncodedFormEntity(var, HTTP.UTF_8);
                    httpPost.setEntity(urlEncodedFormEntity);
                    httpResponse = new DefaultHttpClient().execute(httpPost);
                    Log.e("CASE: ", "insert success");
                }catch(Exception e){
                    Log.e("ERROR: ", e.toString());
                }
            }
        });
        t.start();
        //t.interrupt();
    }

    private void update_to_database(final String id, final String name, final String sex, final String age, final String height, final String weight, final String bmr){
        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                String url = "http://10.0.60.21/android_use/update_mysql.php";
                //String url = "http://192.168.66.18/android_use/update_mysql.php";
                //use post method
                HttpPost httpPost = new HttpPost(url);
                HttpResponse httpResponse;
                List<NameValuePair> var = new ArrayList<NameValuePair>();
                var.add(new BasicNameValuePair("id", id));
                var.add(new BasicNameValuePair("name", name));
                var.add(new BasicNameValuePair("sex", sex));
                var.add(new BasicNameValuePair("age", age));
                var.add(new BasicNameValuePair("height", height));
                var.add(new BasicNameValuePair("weight", weight));
                var.add(new BasicNameValuePair("bmr", bmr));
                try{
                    UrlEncodedFormEntity urlEncodedFormEntity = new UrlEncodedFormEntity(var, HTTP.UTF_8);
                    httpPost.setEntity(urlEncodedFormEntity);
                    httpResponse = new DefaultHttpClient().execute(httpPost);
                    Log.e("CASE: ", "update success");
                }catch(Exception e){
                    Log.e("ERROR: ", e.toString());
                }
            }
        });
        t.start();
        //t.interrupt();
    }
}