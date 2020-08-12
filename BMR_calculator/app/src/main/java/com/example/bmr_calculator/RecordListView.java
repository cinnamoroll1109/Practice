package com.example.bmr_calculator;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

public class RecordListView extends ArrayAdapter<String> {
    private String[] names;
    private Activity context;
    public RecordListView(Activity context, String[] names) {
        super(context, R.layout.layout, names);
        this.context = context;
        this.names = names;
    }

    @NonNull
    @Override
    public View getView(int postition, @Nullable View convertView, @NonNull ViewGroup parent){
        View r = convertView;
        ViewHolder viewHolder = null;
        if(r == null){
            LayoutInflater layoutInflater = context.getLayoutInflater();
            r = layoutInflater.inflate(R.layout.layout, null, true);
            viewHolder = new ViewHolder(r);
            r.setTag(viewHolder);
        }
        else{
            viewHolder = (ViewHolder)r.getTag();
        }

        viewHolder.tv1.setText(names[postition]);

        return r;
    }

    class ViewHolder{
        TextView tv1;

        ViewHolder(View v){
            tv1 = (TextView)v.findViewById(R.id.tvNames);
        }
    }


}
