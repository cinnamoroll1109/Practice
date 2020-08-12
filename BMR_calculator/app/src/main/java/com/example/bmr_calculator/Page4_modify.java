package com.example.bmr_calculator;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.text.SpannableString;
import android.text.Spanned;
import android.text.SpannedString;
import android.text.style.AbsoluteSizeSpan;
import android.util.Log;
import android.view.View;
import android.widget.*;
import android.os.Bundle;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.protocol.HTTP;

import java.util.ArrayList;
import java.util.List;

public class Page4_modify extends AppCompatActivity {
    //宣告物件
    private EditText etName;
    private Spinner spSex;
    private EditText etAge;
    private EditText etHeight;
    private EditText etWeight;
    private EditText etBmr;
    private Button btnBack;
    private Button btnEdit;
    private Button btnDelete;

    String id ;
    String name;
    String sex;
    String age;
    String height;
    String weight;
    String bmr;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_page4_modify);

        etName = findViewById(R.id.p4_etName);
        spSex = findViewById(R.id.p4_spSex);
        etAge = findViewById(R.id.p4_etAge);
        etHeight = findViewById(R.id.p4_etHeight);
        etWeight = findViewById(R.id.p4_etWeight);
        etBmr = findViewById(R.id.p4_etBmr);
        btnBack = findViewById(R.id.p4_btnBack);
        btnEdit = findViewById(R.id.p4_btnEdit);
        btnDelete = findViewById(R.id.p4_btnDelete);

        ArrayAdapter<String> spinner_list = new ArrayAdapter<>(Page4_modify.this,
                android.R.layout.simple_spinner_dropdown_item, new String[]{"男", "女"});
        spSex.setAdapter(spinner_list);

        //取得bundle資料
        final Bundle bundle = getIntent().getExtras();
        id = bundle.getString("id");
        name = bundle.getString("name");
        sex = bundle.getString("sex");
        age = bundle.getString("age");
        height = bundle.getString("height");
        weight = bundle.getString("weight");
        bmr = bundle.getString("bmr");

        setEditTextHintSize(etName, name, 30);
        if(sex == "man"){
            spSex.setSelection(0,true);
        }
        else{
            spSex.setSelection(1, true);
        }
        setEditTextHintSize(etAge, age, 30);
        setEditTextHintSize(etHeight, height, 30);
        setEditTextHintSize(etWeight, weight, 30);
        setEditTextHintSize(etBmr, bmr, 30);

        //back to main page
        View.OnClickListener btnBackOnClick = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //切換頁面
                Intent it = new Intent();
                it.setClass(Page4_modify.this, MainActivity.class);
                startActivity(it);
            }
        };
        btnBack.setOnClickListener(btnBackOnClick);

        //edit data
        View.OnClickListener btnEditOnClick = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Bundle bundle_send = new Bundle();
                bundle_send.putString("insert_or_update", "update");
                bundle_send.putString("id", id);
                if(!("".equals(etName.getText().toString()))){
                    bundle_send.putString("name", etName.getText().toString());
                }
                else{
                    bundle_send.putString("name", name);
                }
                if(spSex.getSelectedItemPosition() == 0){
                    bundle_send.putString("sex", "man");
                }
                else{
                    bundle_send.putString("sex", "woman");
                }
                if(!("".equals(etAge.getText().toString()))){
                    bundle_send.putString("age", etAge.getText().toString());
                }
                else{
                    bundle_send.putString("age", age);
                }
                if(!("".equals(etHeight.getText().toString()))){
                    bundle_send.putString("height", etHeight.getText().toString());
                }
                else{
                    bundle_send.putString("height", height);
                }
                if(!("".equals(etWeight.getText().toString()))){
                    bundle_send.putString("weight", etWeight.getText().toString());
                }
                else{
                    bundle_send.putString("weight", weight);
                }

                //切換頁面
                Intent it = new Intent();
                it.putExtras(bundle_send);
                it.setClass(Page4_modify.this, Page3_output.class);
                startActivity(it);
            }
        };
        btnEdit.setOnClickListener(btnEditOnClick);

        //delete data
        View.OnClickListener btnDeleteOnClick = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                delete_from_database(id);
                //切換頁面
                Intent it = new Intent();
                it.setClass(Page4_modify.this, MainActivity.class);
                startActivity(it);
            }
        };
        btnDelete.setOnClickListener(btnDeleteOnClick);
    }

    public static void setEditTextHintSize(EditText editText, String hintContent, int hintSize) {
        // 设置hint字体大小
        SpannableString ss = new SpannableString(hintContent);
        AbsoluteSizeSpan ass = new AbsoluteSizeSpan(hintSize, true);
        ss.setSpan(ass, 0, ss.length(), Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        // 设置hint
        editText.setHint(new SpannedString(ss)); // 一定要进行转换,否则属性会消失
    }

    private void delete_from_database(String pid){
        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                String url = "http://10.0.60.21/android_use/delete_row.php";
                //String url = "http://192.168.66.18/android_use/delete_row.php";
                //use post method
                HttpPost httpPost = new HttpPost(url);
                HttpResponse httpResponse;
                List<NameValuePair> var = new ArrayList<NameValuePair>();
                var.add(new BasicNameValuePair("id", id));
                //var.add(new BasicNameValuePair("name", name));
                //var.add(new BasicNameValuePair("sex", sex));
                //var.add(new BasicNameValuePair("age", age));
                //var.add(new BasicNameValuePair("height", height));
                //var.add(new BasicNameValuePair("weight", weight));
                //var.add(new BasicNameValuePair("bmr", bmr));
                try{
                    UrlEncodedFormEntity urlEncodedFormEntity = new UrlEncodedFormEntity(var, HTTP.UTF_8);
                    httpPost.setEntity(urlEncodedFormEntity);
                    httpResponse = new DefaultHttpClient().execute(httpPost);
                    Log.e("CASE: ", "delete success");
                }catch(Exception e){
                    Log.e("ERROR: ", e.toString());
                }
            }
        });
        t.start();
        //t.interrupt();
    }
}